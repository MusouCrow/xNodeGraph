using UnityEngine;
using XNode;

namespace Game.Graph {
    public class FlowNode : BaseNode {
        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        public Solt In;

        [Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        public Solt Out;
    }
}