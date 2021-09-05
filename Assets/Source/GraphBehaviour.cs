using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    using Graph;

    public class GraphBehaviour : MonoBehaviour {
        public BaseGraph graph;

        private Runtime runtime;

        protected void Start() {
            this.runtime = new Runtime(this.graph);
            this.runtime.RunFunc("Init");
        }

        protected void Update() {
            
        }
    }
}