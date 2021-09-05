using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class LogNode : FlowNode {
        public override string Title {
            get {
                return "调试输出";
            }
        }

        public override string Note {
            get {
                return @"打印输出结果";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public string value;
        private BaseNode valueNode;

        protected override void Init() {
            base.Init();

            this.valueNode = this.GetPortNode("value");
        }

        public override object Run(Runtime runtime) {
            var value = this.GetValue<object>(this.value, this.valueNode, runtime);
            Debug.Log(value.ToString());

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var value = await this.GetValueAsync<object>(this.value, this.valueNode, runtime);
            Debug.Log(value.ToString());

            return null;
        }
    }
}