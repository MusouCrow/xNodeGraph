using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using Game.Graph;

namespace GEditor.Graph {
    public static class GenerateNodes {
        private const string CODE = @"using System.Threading.Tasks;
using Game.Graph;

namespace Generated.Graph.{Namespace} {
    [CreateNodeMenuAttribute('{Title}')]
    public class {ClassName} : {ParentNode} {
        public override string Title {
            get {
                return '{Title}';
            }
        }

        public override string Note {
            get {
                return '{Note}';
            }
        }

{Defines}

        protected override void Init() {
            base.Init();
            
{Init}
        }

        public override object Run(Runtime runtime, int id) {
{Call}
            return {ReturnRun};
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
{CallAsync}
            return {ReturnRun};
        }
    }
}
        ";

        private const string DEFINE_INPUT_CODE = @"[Input(connectionType = ConnectionType.Override)]
        public {Type} {Name};
        private BaseNode {Name}Node;
        ";

        private const string DEFINE_OUTPUT_CODE = "[Output]public {Type} ret;";
        private const string INIT_CODE = "this.{Name}Node = this.GetPortNode(\"{Name}\");";
        private const string GET_VALUE_CODE = "var {Name} = this.GetValue<{Type}>(this.{Name}, this.{Name}Node, runtime);";
        private const string GET_VALUE_ASYNC_CODE = "var {Name} = await this.GetValueAsync<{Type}>(this.{Name}, this.{Name}Node, runtime);";
        private const string GET_OBJECT_CODE = "var {Name} = this.GetObject(this.{Name}Node, runtime);";
        private const string GET_OBJECT_ASYNC_CODE = "var {Name} = await this.GetObjectAsync(this.{Name}Node, runtime);";
        private const string CACHE_VALUE_CODE = "runtime.CacheValue(this, this.ret);";
        private const string GET_VAR_CODE = "var {Name} = runtime.variable[\"{Name}\"] as {Type};";
        private const string AWAIT_CODE = "await Task.CompletedTask;";

        public const string GEN_PATH = "Assets/Generated/Graph/";

        public static Dictionary<Type, Type> TypeMapping = new Dictionary<Type, Type>() {
            {typeof(System.Single), typeof(Number)},
            {typeof(System.Int32), typeof(Number)},
            {typeof(System.Boolean), typeof(Bool)},
            {typeof(UnityEngine.Vector3), typeof(Vec3)},
            {typeof(System.Object), typeof(Obj)},
            {typeof(UnityEngine.Color), typeof(Col)},
        };

        [MenuItem("Tools/Graph/Generate Nodes")]
        public static void Menu() {
            ClearFolder();
            Generates();
            EditorUtility.RequestScriptReload();
        }

        private static void ClearFolder() {
            if (!Directory.Exists(GEN_PATH)) {
                return;
            }

            Directory.Delete(GEN_PATH, true);
        }

        private static void Generates() {
            var types = typeof(NodeAttribute).Assembly.GetExportedTypes();

            foreach (var t in types) {
                var methods = t.GetMethods(BindingFlags.Static | BindingFlags.Public);
                
                foreach (var m in methods) {
                    var attr = m.GetCustomAttribute<NodeAttribute>();

                    if (attr != null) {
                        GenerateNode(t, m, attr);
                    }
                }
            }
        }

        private static void GenerateNode(Type type, MethodInfo method, NodeAttribute attr) {
            var code = GenerateCodeBody("G" + type.FullName, method.Name + "Node", attr.title, attr.note, attr.isFlow);
            var defines = GenerateDefinesCode(method, attr);
            var init = GenerateInitCode(method, attr);
            var call = GenerateCallCode(type, method, attr, false);
            var callAsync = GenerateCallCode(type, method, attr, true);
            var ret = GenerateReturnCode(method.ReturnType);
            
            code = code.Replace("{Defines}", defines);
            code = code.Replace("{Init}", init);
            code = code.Replace("{Call}", call);
            code = code.Replace("{CallAsync}", callAsync);
            code = code.Replace("{ReturnRun}", ret);

            WriteCode(type.FullName, method.Name + "Node", code.ToString());
        }

        private static StringBuilder GenerateCodeBody(string nameSpace, string className, string title, string note, bool isFlow) {
            var sb = new StringBuilder(CODE);
            sb = sb.Replace("'", "\"");
            sb = sb.Replace("{Namespace}", nameSpace);
            sb = sb.Replace("{ClassName}", className);
            sb = sb.Replace("{ParentNode}", isFlow ? "FlowNode" : "BaseNode");
            sb = sb.Replace("{Title}", title);
            sb = sb.Replace("{Note}", note);

            return sb;
        }

        private static string GenerateDefinesCode(MethodInfo method, NodeAttribute attr) {
            var sb = new StringBuilder();

            foreach (var p in method.GetParameters()) {
                if (attr.ingores.Contains(p.Name)) {
                    continue;
                }

                var code = "\t\t" + DEFINE_INPUT_CODE;
                var type = ConvertType(p.ParameterType);

                code = code.Replace("{Type}", type);
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine(code);
            }
            
            if (!IsVoidType(method.ReturnType)) {
                var code = "\t\t" + DEFINE_OUTPUT_CODE;
                var type = ConvertType(method.ReturnType);

                code = code.Replace("{Type}", type);
                sb.Append(code);
            }

            return sb.ToString();
        }

        private static string GenerateInitCode(MethodInfo method, NodeAttribute attr) {
            var sb = new StringBuilder();

            foreach (var p in method.GetParameters()) {
                if (attr.ingores.Contains(p.Name)) {
                    continue;
                }

                var code = INIT_CODE;
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine("\t\t\t" + code);
            }

            return sb.ToString();
        }

        private static string GenerateCallCode(Type type, MethodInfo method, NodeAttribute attr, bool async) {
            var sb = new StringBuilder();
            var pars = method.GetParameters();
            var hasRet = !IsVoidType(method.ReturnType);
            var asyncAttr = method.GetCustomAttribute<AsyncStateMachineAttribute>();

            if (asyncAttr != null && !async) {
                return "";
            }

            foreach (var p in pars) {
                string code;
                bool isObj = p.ParameterType == typeof(object);

                if (attr.ingores.Contains(p.Name)) {
                    code = GET_VAR_CODE;
                }
                else if (async) {
                    code = isObj ? GET_OBJECT_ASYNC_CODE : GET_VALUE_ASYNC_CODE;
                }
                else {
                    code = isObj ? GET_OBJECT_CODE : GET_VALUE_CODE;
                }

                var t = ConvertType(p.ParameterType, true);
                code = code.Replace("{Type}", t);
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine("\t\t\t" + code);
            }

            sb.Append("\t\t\t");
            
            if (hasRet) {
                if (IsValueType(method.ReturnType)) {
                    sb.Append("this.ret.value = ");
                }
                else {
                    sb.Append("this.ret = ");
                }
            }

            if (asyncAttr != null && async) {
                sb.Append("await ");
            }

            sb.Append(type.FullName + "." + method.Name);
            sb.Append("(");

            for (int i = 0; i < pars.Length; i++) {
                if (pars[i].ParameterType == typeof(System.Int32)) {
                    sb.Append("(int)");
                }

                sb.Append(pars[i].Name);
                
                if (IsValueType(pars[i].ParameterType, true)) {
                    sb.Append(".value");
                }

                if (i < pars.Length - 1) {
                    sb.Append(", ");
                }
            }

            sb.Append(");");
            sb.AppendLine();

            if (attr.isFlow && hasRet) {
                sb.AppendLine("\t\t\t" + CACHE_VALUE_CODE);
            }

            if (asyncAttr == null && async) {
                sb.AppendLine("\t\t\t" + AWAIT_CODE);
            }

            return sb.ToString();
        }

        private static string GenerateReturnCode(Type type) {
            string ret;
            
            if (IsVoidType(type)) {
                ret = "null";
            }
            else if (type == typeof(object)) {
                ret = "this.ret.value";
            }
            else {
                ret = "this.ret";
            }

            return ret;
        }

        private static void WriteCode(string nameSpace, string className, string code) {
            var path = nameSpace.Replace(".", "/");
            path = GEN_PATH + path;

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            path = path + "/" + className + ".cs";
            File.WriteAllText(path, code);
            Debug.Log("Write to " + path);
        }

        private static bool IsValueType(Type type, bool convertObj=false) {
            var gtypes = type.GenericTypeArguments;

            if (gtypes != null && gtypes.Length > 0 && type.BaseType == typeof(Task)) {
                type = gtypes[0];
            }

            bool cond = convertObj ? type != typeof(object) : true;

            return TypeMapping.ContainsKey(type) && cond;
        }

        private static string ConvertType(Type type, bool convertObj=false) {
            var gtypes = type.GenericTypeArguments;

            if (gtypes != null && gtypes.Length > 0) {
                if (type.BaseType == typeof(Task)) {
                    type = gtypes[0];
                }
                else {
                    return ConvertGenericType(type, convertObj);
                }
            }

            if (TypeMapping.ContainsKey(type)) {
                type = TypeMapping[type];
            }

            if (convertObj && type == typeof(Obj)) {
                type = typeof(object);
            }

            var name = type.FullName;
            name = name.Replace("+", ".");

            return name;
        }

        private static string ConvertGenericType(Type type, bool convertObj=false) {
            var gtypes = type.GenericTypeArguments;

            var sb = new StringBuilder();
            sb.Append(type.BaseType.FullName);
            sb.Append("<");

            for (int i = 0; i < gtypes.Length; i++) {
                var t = ConvertType(gtypes[i], convertObj);
                sb.Append(t);

                if (i < gtypes.Length - 1) {
                    sb.Append(", ");
                }
            }

            sb.Append(">");

            return sb.ToString();
        }

        private static bool IsVoidType(Type type) {
            if (type.FullName == "System.Void") {
                return true;
            }

            var gtypes = type.GenericTypeArguments;

            return type == typeof(Task) && (gtypes == null || gtypes.Length == 0);
        }
    }
}