using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Graph {
    public class LogNode : FlowNode {
        [Input(connectionType = ConnectionType.Override)]
        public string value;

        private BaseNode valueNode;

        protected override void Init() {
            this.valueNode = this.GetPortNode("value");
        }

        public override object Run(Dictionary<BaseNode, object> cache) {
            var value = this.GetValue<string>(this.value, this.valueNode, cache);
            Debug.Log(value);

            return null;
        }

        public async override Task<object> RunAsync(Dictionary<BaseNode, object> cache) {
            var value = this.GetValueAsync<string>(this.value, this.valueNode, cache);
            Debug.Log(value.Result);

            return null;
        }
    }
}