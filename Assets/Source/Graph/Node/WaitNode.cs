using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("流程-等待")]
    public class WaitNode : FlowNode {
        public override string Title {
            get {
                return "流程-等待";
            }
        }

        public override string Note {
            get {
                return @"等待条件达成后继续执行";
            }
        }

        public override bool Async {
            get {
                return true;
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Bool condition;
        private BaseNode conditionNode;

        protected override void Init() {
            base.Init();

            this.conditionNode = this.GetPortNode("condition");
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            while (true) {
                var condition = await this.GetValueAsync<Bool>(this.condition, this.conditionNode, runtime);

                if (condition.value) {
                    break;
                }
                else {
                    await Task.Yield();
                }
            }

            return null;
        }
    }
}