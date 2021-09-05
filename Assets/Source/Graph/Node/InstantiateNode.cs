using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class InstantiateNode : FlowNode {
        public override string Title {
            get {
                return "创建GameObject";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public GameObject prefab;
        private BaseNode prefabNode;

        [Output]
        public GameObject ret;

        protected override void Init() {
            base.Init();
            this.prefabNode = this.GetPortNode("prefab");
        }

        public override object Run(Runtime runtime) {
            var prefab = this.GetValue<GameObject>(this.prefab, this.prefabNode, runtime);
            var go = GameObject.Instantiate(prefab);
            runtime.CacheValue(this, go);

            return go;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var prefab = await this.GetValueAsync<GameObject>(this.prefab, this.prefabNode, runtime);
            var go = GameObject.Instantiate(prefab);
            runtime.CacheValue(this, go);

            return go;
        }
    }
}