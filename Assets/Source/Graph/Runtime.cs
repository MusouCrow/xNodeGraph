using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game.Graph {
    public class Runtime {
        private BaseGraph graph;
        private HashSet<int> exitIdSet;
        public Dictionary<BaseNode, object> cache;
        public Dictionary<string, object> variable;

        public Runtime(BaseGraph graph) {
            this.graph = graph;
            this.cache = new Dictionary<BaseNode, object>();
            this.variable = new Dictionary<string, object>();
            this.exitIdSet = new HashSet<int>();
        }

        public async void RunFunc(string func, int id=0, BaseGraph graph=null) {
            graph = graph == null ? this.graph : graph;

            if (!graph.funcMap.ContainsKey(func)) {
                return;
            }

            var f = graph.funcMap[func];
            id = id > 0 ? id : func.GetHashCode();

            if (id > 0 && this.exitIdSet.Contains(id)) {
                this.exitIdSet.Remove(id);
            }
            
            if (f.async) {
                await this.RunNodeAsync(f.node, id);
            }
            else {
                this.RunNode(f.node, id);
            }
        }

        public async Task RunFuncWaitting(string func, int id=0, BaseGraph graph=null) {
            graph = graph == null ? this.graph : graph;

            if (!graph.funcMap.ContainsKey(func)) {
                return;
            }

            var f = graph.funcMap[func];
            id = id > 0 ? id : func.GetHashCode();
            
            if (f.async) {
                await this.RunNodeAsync(f.node, id);
            }
            else {
                this.RunNode(f.node, id);
            }
        }

        public void CacheValue(BaseNode node, object value) {
            if (node.NextNode) {
                this.cache[node] = value;
            }
        }

        public void RunNode(BaseNode node, int id=0) {
            if (this.exitIdSet.Contains(id)) {
                return;
            }

            node.Run(this, id);

            var next = node.NextNode;
            
            if (next) {
                this.RunNode(next, id);
            }
        }

        public async Task RunNodeAsync(BaseNode node, int id=0) {
            if (this.exitIdSet.Contains(id)) {
                return;
            }

            await node.RunAsync(this, id);

            var next = node.NextNode;
            
            if (next) {
                await this.RunNodeAsync(next, id);
            }
        }

        public void SetVariable(string name, object value) {
            if (value != null) {
                this.variable[name] = value;
            }
        }

        public void ExitFunc(int id) {
            if (id > 0) {
                this.exitIdSet.Add(id);
            }
        }

        public void ExitFunc(string func) {
            this.ExitFunc(func.GetHashCode());
        }
    }
}