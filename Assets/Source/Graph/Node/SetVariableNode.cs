using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("设置变量")]
    public class SetVariableNode : FlowNode {
        public override string Title {
            get {
                return "设置变量";
            }
        }

        public override string Note {
            get {
                return @"通过名字与值设置变量";
            }
        }

        public string var;

        [Input(connectionType = ConnectionType.Override)]
        public Obj value;
        private BaseNode valueNode;

        protected override void Init() {
            base.Init();
            this.valueNode = this.GetPortNode("value");
        }

        public override object Run(Runtime runtime, int id) {
            var value = this.GetObject(this.valueNode, runtime);
            this.SetValue(runtime, value);

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var value = await this.GetObjectAsync(this.valueNode, runtime);
            this.SetValue(runtime, value);

            return null;
        }

        private void SetValue(Runtime runtime, object value) {
            if (this.TrySetValue<Number, float>(runtime, value)) {

            }
            else if (this.TrySetValue<Bool, bool>(runtime, value)) {

            }
            else if (this.TrySetValue<Vec3, Vector3>(runtime, value)) {
                
            }
            else if (this.TrySetValue<Col, Color>(runtime, value)) {
                
            }
            else if (this.TrySetValue<Quat, Quaternion>(runtime, value)) {
                
            }
            else {
                runtime.variable[this.var] = value;
            }
        }

        private bool TrySetValue<T, U>(Runtime runtime, object value) where T : Variables<U>, new() {
            var variable = runtime.variable;

            if (value is T) {
                var v = value as T;
                
                if (!variable.ContainsKey(this.var) || !(variable[this.var] is T)) {
                    variable[this.var] = new T();
                }

                var vv = variable[this.var] as T;
                vv.value = v.value;

                return true;
            }

            return false;
        }
    }
}