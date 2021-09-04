using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class DelayNode : FlowNode {
        [Input(connectionType = ConnectionType.Override)]
        public Float time;
        private BaseNode timeNode;

        protected override void Init() {
            base.Init();
            this.timeNode = this.GetPortNode("time");
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var time = await this.GetValueAsync<Float>(this.time, this.timeNode, runtime);
            await Task.Delay(Mathf.CeilToInt(time.value * 1000));

            return null;
        }
    }
}