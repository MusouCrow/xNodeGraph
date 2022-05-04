using System.Threading.Tasks;

namespace Game.Graph {
    [CreateNodeMenuAttribute("调用蓝图")]
    public class CallGraphNode : FlowNode {
        public override string Title {
            get {
                return "调用蓝图";
            }
        }

        public override string Note {
            get {
                return "调用其他蓝图内的功能节点";
            }
        }

        public BaseGraph baseGraph;
        public string func;
        public bool watting;

        public override object Run(Runtime runtime, int id) {
            var func = this.func == "" ? "Main" : this.func;
            runtime.RunFunc(func, id, this.baseGraph);

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var func = this.func == "" ? "Main" : this.func;

            if (watting) {
                await runtime.RunFuncWaitting(func, id, this.baseGraph);
            }
            else {
                runtime.RunFunc(func, id, this.baseGraph);
            }

            return null;
        }
    }
}