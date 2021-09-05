using UnityEngine;
using XNode;

namespace Game.Graph {
    public class FuncNode : BaseNode {
        public override string Title {
            get {
                return "功能";
            }
        }
        
        [Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        public Solt Out;

        public string func;
    }
}