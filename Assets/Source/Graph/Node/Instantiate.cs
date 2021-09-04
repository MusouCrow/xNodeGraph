using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class InstantiateNode : FlowNode {
        [Input(connectionType = ConnectionType.Override)]
        public GameObject prefab;
        private BaseNode prefabNode;

        [Output]
        public GameObject ret;

        protected override void Init() {
            this.prefabNode = this.GetPortNode("prefab");
        }

        public override object Run(Runtime runtime) {
            var prefab = this.GetValue<GameObject>(this.prefab, this.prefabNode, runtime);
            var go = GameObject.Instantiate(prefab);
            runtime.cache[this] = go;

            return go;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var prefab = this.GetValueAsync<GameObject>(this.prefab, this.prefabNode, runtime).Result;
            var go = GameObject.Instantiate(prefab);
            runtime.cache[this] = go;

            return go;
        }
    }
}