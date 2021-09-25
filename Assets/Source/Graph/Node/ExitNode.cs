using System.Threading.Tasks;

namespace Game.Graph {
    [CreateNodeMenuAttribute("结束")]
    public class ExitNode : FlowNode {
        public override string Title {
            get {
                return "结束";
            }
        }

        public override string Note {
            get {
                return "结束本次执行过程";
            }
        }

        public override object Run(Runtime runtime, int id) {
            runtime.ExitFunc(id);

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            await Task.CompletedTask;
            runtime.ExitFunc(id);

            return null;
        }
    }
}

