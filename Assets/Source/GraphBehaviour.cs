using UnityEngine;

namespace Game.Component {
    using Graph;
    
    public class GraphBehaviour : MonoBehaviour {
        public BaseGraph graph;

        [SerializeField]
        private Blackboard blackboard = new Blackboard();
        private Runtime runtime;

        protected void Awake() {
            this.runtime = new Runtime(this.graph, this.blackboard);
            this.runtime.RunFunc("Awake");
        }

        protected void Start() {
            this.runtime.RunFunc("Start");
        }

        protected void OnDestroy() {
            this.runtime.RunFunc("Destroy");
            this.runtime.Exit();
        }

        protected void FixedUpdate() {
            this.runtime.RunFunc("Update");
        }

        public void RunFunc(string func) {
            this.runtime.RunFunc(func);
        }
    }
}