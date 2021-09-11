using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game.Graph {
    public class Runtime {
        private BaseGraph graph;
        private Dictionary<string, FuncNode> funcMap;
        public Dictionary<BaseNode, object> cache;
        public Dictionary<string, object> variable;

        public Runtime(BaseGraph graph) {
            this.graph = graph;
            this.cache = new Dictionary<BaseNode, object>();
            this.variable = new Dictionary<string, object>();
            this.InitFuncMap();
        }

        private void InitFuncMap() {
            this.funcMap = new Dictionary<string, FuncNode>();

            foreach (var node in this.graph.nodes) {
                if (node is FuncNode) {
                    var funcNode = node as FuncNode;
                    this.funcMap.Add(funcNode.func, funcNode);
                }
            }
        }

        public void RunFunc(string func) {
            if (!this.funcMap.ContainsKey(func)) {
                return;
            }

            this.RunNode(this.funcMap[func]);
        }

        public async void RunFuncAsync(string func) {
            if (!this.funcMap.ContainsKey(func)) {
                return;
            }
            
            await this.RunNodeAsync(this.funcMap[func]);
        }

        public void CacheValue(BaseNode node, object value) {
            if (node.NextNode) {
                this.cache[node] = value;
            }
        }

        public void RunNode(BaseNode node) {
            node.Run(this);

            var next = node.NextNode;
            
            if (next) {
                this.RunNode(next);
            }
        }

        public async Task RunNodeAsync(BaseNode node) {
            await node.RunAsync(this);

            var next = node.NextNode;
            
            if (next) {
                await this.RunNodeAsync(next);
            }
        }

        public void SetVariable(string name, object value) {
            if (value != null) {
                this.variable[name] = value;
            }
        }
    }
}