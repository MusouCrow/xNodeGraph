using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    using Graph;

    public class GraphBehaviour : MonoBehaviour {
        public BaseGraph graph;
        public Blackboard blackboard;

        private Runtime runtime;

        protected void Start() {
            this.runtime = new Runtime(this.graph, this.blackboard);
            this.runtime.RunFunc("Init");
        }

        protected void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                this.runtime.RunFunc("Enter");
            }
        }
    }
}