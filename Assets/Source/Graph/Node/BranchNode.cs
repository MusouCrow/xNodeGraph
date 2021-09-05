using System.Threading.Tasks;

namespace Game.Graph {
    public class BranchNode : FlowNode {
        public override string Title {
            get {
                return "条件分支";
            }
        }

        public override string Note {
            get {
                return "根据condition决定代码接下来的走向(True, False)，最后再走Out";
            }
        }
        
        [Input(connectionType = ConnectionType.Override)]
        public Bool condition;
        private BaseNode conditionNode;

        [Output]
        public Solt True;
        private BaseNode TrueNode;

        [Output]
        public Solt False;
        private BaseNode FalseNode;

        protected override void Init() {
            base.Init();

            this.conditionNode = this.GetPortNode("condition");
            this.TrueNode = this.GetPortNode("True");
            this.FalseNode = this.GetPortNode("False");
        }

        public override object Run(Runtime runtime) {
            var condition = this.GetValue<Bool>(this.condition, this.conditionNode, runtime);
            
            if (condition.value) {
                this.TrueNode.Run(runtime);
            }
            else {
                this.FalseNode.Run(runtime);
            }

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var condition = await this.GetValueAsync<Bool>(this.condition, this.conditionNode, runtime);

            if (condition.value) {
                this.TrueNode.Run(runtime);
            }
            else {
                this.FalseNode.Run(runtime);
            }

            return null;
        }
    }
}

