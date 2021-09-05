using System.Threading.Tasks;
using XNode;

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

        protected override void Init() {
            base.Init();
            this.funcNode = this.GetPortNode("func");
        }

        public override object Run(Runtime runtime) {
            var func = this.GetValue<string>(this.func, this.funcNode, runtime);
            runtime.RunFunc(func);

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var func = await this.GetValueAsync<string>(this.func, this.funcNode, runtime);
            runtime.RunFuncAsync(func);

            return null;
        }
    }
}