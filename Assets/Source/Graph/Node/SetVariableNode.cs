using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class SetVariableNode : FlowNode {
        public string var;

        [Input(connectionType = ConnectionType.Override)]
        public Obj value;
        private BaseNode valueNode;

        protected override void Init() {
            base.Init();
            this.valueNode = this.GetPortNode("value");
        }

        public override object Run(Runtime runtime) {
            var value = this.GetValue<object>(this.value, this.valueNode, runtime);
            runtime.variable[this.var] = value;

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var value = await this.GetValueAsync<object>(this.value, this.valueNode, runtime);
            runtime.variable[this.var] = value;

            return null;
        }
    }
}