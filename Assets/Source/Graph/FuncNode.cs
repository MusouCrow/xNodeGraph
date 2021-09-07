namespace Game.Graph {
    [CreateNodeMenuAttribute("功能")]
    public class FuncNode : BaseNode {
        public override string Title {
            get {
                return "功能";
            }
        }

        public override string Note {
            get {
                return "将一系列节点封装成功能的节点";
            }
        }
        
        [Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        public Solt Out;

        public string func;
    }
}