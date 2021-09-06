using System;

namespace Game.Graph {
    public class NodeAttribute : Attribute {
        public string title;
        public string note;
        public bool isFlow;

        public NodeAttribute(string title, string note=null, bool isFlow=true) {
            this.title = title;
            this.note = note;
            this.isFlow = isFlow;
        }
    }
}