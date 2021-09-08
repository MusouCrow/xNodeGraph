using System.Collections.Generic;
using System;

namespace Game.Graph {
    public class NodeAttribute : Attribute {
        public string title;
        public string note;
        public bool isFlow;
        public HashSet<string> ingores;

        public NodeAttribute(string title, string note=null, bool isFlow=true, params string[] ingores) {
            this.title = title;
            this.note = note;
            this.isFlow = isFlow;
            this.ingores = new HashSet<string>();

            foreach (var s in ingores) {
                this.ingores.Add(s);
            }
        }
    }
}