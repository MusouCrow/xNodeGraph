using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("流程-等待一帧")]
    public class WaitFrameNode : FlowNode {
        public override string Title {
            get {
                return "流程-等待一帧";
            }
        }

        public override string Note {
            get {
                return @"等待一帧后继续";
            }
        }

        public override bool Async {
            get {
                return true;
            }
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            await Task.Yield();

            return null;
        }
    }
}