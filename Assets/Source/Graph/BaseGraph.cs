using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateAssetMenu(menuName = "Graph/Base")]
    public class BaseGraph : NodeGraph {
        public struct Func {
            public FuncNode node;
            public bool async;
        }

        public Dictionary<string, Func> funcMap;

        protected void OnEnable() {
            this.funcMap = new Dictionary<string, Func>();

            foreach (var node in this.nodes) {
                var n = node as FuncNode;

                if (n) {
                    bool async = this.IsAsyncNode(n);
                    this.funcMap.Add(n.func, new Func() {node = n, async = async});
                }
            }
        }

        private bool IsAsyncNode(BaseNode node) {
            if (node.Async) {
                return true;
            }
            else if (node is CallNode) {
                var callNode = node as CallNode;

                if (callNode.watting) {
                    return true;
                }
            }
            else if (node is CallGraphNode) {
                var callNode = node as CallGraphNode;

                if (callNode.watting) {
                    return true;
                }
            }

            foreach (var port in node.Inputs) {
                if (port.fieldName == "In") {
                    continue;
                }

                var n = node.GetPortNode(port.fieldName);

                if (n && this.IsAsyncNode(n)) {
                    return true;
                }
            }
            
            if (node.NextNode && this.IsAsyncNode(node.NextNode)) {
                return true;
            }

            return false;
        }
    }
}