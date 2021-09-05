using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class DelayNode : FlowNode {
        public override string Title {
            get {
                return "延时";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Number time;
        private BaseNode timeNode;

        protected override void Init() {
            base.Init();
            this.timeNode = this.GetPortNode("time");
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var time = await this.GetValueAsync<Number>(this.time, this.timeNode, runtime);
            await Task.Delay(Mathf.CeilToInt(time.value * 1000));

            return null;
        }
    }
}