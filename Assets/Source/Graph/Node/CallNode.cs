using System.Threading.Tasks;

namespace Game.Graph {
    [CreateNodeMenuAttribute("调用功能")]
    public class CallNode : FlowNode {
        public override string Title {
            get {
                return "调用功能";
            }
        }

        public override string Note {
            get {
                return "调用本蓝图内的其他功能节点";
            }
        }
        
        [Input(connectionType = ConnectionType.Override)]
        public string func;
        private BaseNode funcNode;

        public bool watting;

        protected override void Init() {
            base.Init();
            this.funcNode = this.GetPortNode("func");
        }

        public override object Run(Runtime runtime, int id) {
            var func = this.GetValue<string>(this.func, this.funcNode, runtime);
            runtime.RunFunc(func, id, this.graph as BaseGraph);

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var func = await this.GetValueAsync<string>(this.func, this.funcNode, runtime);
            
            if (watting) {
                await runtime.RunFuncWaitting(func, id, this.graph as BaseGraph);
            }
            else {
                runtime.RunFunc(func, id);
            }

            return null;
        }
    }
}