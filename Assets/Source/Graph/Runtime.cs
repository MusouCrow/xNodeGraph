using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Graph {
    public class Runtime {
        private BaseGraph graph;
        private Dictionary<string, FuncNode> funcMap;
        private Dictionary<BaseNode, object> cache;

        public Runtime(BaseGraph graph) {
            this.graph = graph;
            this.cache = new Dictionary<BaseNode, object>();
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

            this.cache.Clear();
            this.RunNode(this.funcMap[func], this.cache);
        }

        public async void RunFuncAsync(string func) {
            if (!this.funcMap.ContainsKey(func)) {
                return;
            }

            this.cache.Clear();
            await this.RunNodeAsync(this.funcMap[func], this.cache);
        }

        private void RunNode(BaseNode node, Dictionary<BaseNode, object> cache) {
            node.Run(cache);

            var next = this.GetNextNode(node);
            
            if (next) {
                this.RunNode(next, cache);
            }
        }

        private async Task RunNodeAsync(BaseNode node, Dictionary<BaseNode, object> cache) {
            await node.RunAsync(cache);

            var next = this.GetNextNode(node);
            
            if (next) {
                await this.RunNodeAsync(next, cache);
            }
        }

        private BaseNode GetNextNode(BaseNode node) {
            var port = node.GetPort("Out");
            BaseNode next = null;

            if (port.IsConnected) {
                next = port.GetConnection(0).node as BaseNode;
            }

            return next;
        }
    }
}