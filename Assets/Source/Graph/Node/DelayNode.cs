using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("流程-延时")]
    public class DelayNode : FlowNode {
        public override string Title {
            get {
                return "流程-延时";
            }
        }

        public override string Note {
            get {
                return @"等待一段时间后继续执行";
            }
        }

        public override bool Async {
            get {
                return true;
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Number time;
        private BaseNode timeNode;

        protected override void Init() {
            base.Init();
            this.timeNode = this.GetPortNode("time");
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var time = await this.GetValueAsync<Number>(this.time, this.timeNode, runtime);
            await Task.Delay(Mathf.CeilToInt(time.value * 1000));

            return null;
        }
    }
}