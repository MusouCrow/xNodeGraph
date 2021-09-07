using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
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

        public override object Run(Runtime runtime) {
{Call}
            return {ReturnRun};
        }

        public async override Task<object> RunAsync(Runtime runtime) {
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
        private const string CACHE_VALUE_CODE = "runtime.CacheValue(this, ret);";

        public const string GEN_PATH = "Assets/Generated/Graph/";

        public static Dictionary<Type, Type> TypeMapping = new Dictionary<Type, Type>() {
            {typeof(System.Single), typeof(Number)},
            {typeof(System.Int32), typeof(Number)},
            {typeof(System.Boolean), typeof(Bool)}
        };

        [MenuItem("Tools/Graph/Generate Nodes")]
        public static void Menu() {
            ClearFolder();
            Generates();
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
            var code = GenerateCodeBody(type.Name, method.Name + "Node", attr.title, attr.note, attr.isFlow);
            var defines = GenerateDefinesCode(method);
            var init = GenerateInitCode(method);
            var call = GenerateCallCode(type, method, attr, false);
            var callAsync = GenerateCallCode(type, method, attr, true);
            var ret = method.ReturnType.FullName == "System.Void" ? "null" : "this.ret";
            
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

        private static string GenerateDefinesCode(MethodInfo method) {
            var sb = new StringBuilder();

            foreach (var p in method.GetParameters()) {
                var code = "\t\t" + DEFINE_INPUT_CODE;
                var type = ConvertType(p.ParameterType);

                code = code.Replace("{Type}", type.FullName);
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine(code);
            }
            
            if (method.ReturnType.FullName != "System.Void") {
                var code = "\t\t" + DEFINE_OUTPUT_CODE;
                var type = ConvertType(method.ReturnType);

                code = code.Replace("{Type}", type.FullName);
                sb.Append(code);
            }

            return sb.ToString();
        }

        private static string GenerateInitCode(MethodInfo method) {
            var sb = new StringBuilder();

            foreach (var p in method.GetParameters()) {
                var code = INIT_CODE;
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine("\t\t\t" + code);
            }

            return sb.ToString();
        }

        private static string GenerateCallCode(Type type, MethodInfo method, NodeAttribute attr, bool async) {
            var sb = new StringBuilder();
            var pars = method.GetParameters();
            var hasRet = method.ReturnType.FullName != "System.Void";

            foreach (var p in pars) {
                var code = async ? GET_VALUE_ASYNC_CODE : GET_VALUE_CODE;
                var t = ConvertType(p.ParameterType);

                code = code.Replace("{Type}", t.FullName);
                code = code.Replace("{Name}", p.Name);
                sb.AppendLine("\t\t\t" + code);
            }

            sb.Append("\t\t\t");
            
            if (hasRet) {
                if (TypeMapping.ContainsKey(method.ReturnType)) {
                    sb.Append("this.ret.value = ");
                }
                else {
                    sb.Append("this.ret = ");
                }
            }

            sb.Append(type.FullName + "." + method.Name);
            sb.Append("(");

            for (int i = 0; i < pars.Length; i++) {
                sb.Append(pars[i].Name);
                
                if (TypeMapping.ContainsKey(pars[i].ParameterType)) {
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

            return sb.ToString();
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

        private static Type ConvertType(Type type) {
            if (TypeMapping.ContainsKey(type)) {
                return TypeMapping[type];
            }

            return type;
        }
    }
}