using UnityEngine;
using XNode;

namespace Game.Graph {
    public class FuncNode : BaseNode {
        [Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        public Solt Out;

        public string func;
    }
}