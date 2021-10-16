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
            
            var nodeCache = new Dictionary<Node, bool>();
            
            foreach (var node in this.nodes) {
                var n = node as FuncNode;

                if (n) {
                    bool async = this.IsAsyncNode(n, nodeCache);
                    this.funcMap.Add(n.func, new Func() {node = n, async = async});
                }
            }
        }

        private bool IsAsyncNode(BaseNode node, Dictionary<Node, bool> cache) {
            if (cache.ContainsKey(node)) {
                return cache[node];
            }

            if (node.Async) {
                cache.Add(node, true);
                return true;
            }
            else if (node is CallNode) {
                var callNode = node as CallNode;

                if (callNode.watting) {
                    cache.Add(node, true);
                    return true;
                }
            }
            else if (node is CallGraphNode) {
                var callNode = node as CallGraphNode;

                if (callNode.watting) {
                    cache.Add(node, true);
                    return true;
                }
            }
            else if (node is BranchNode) {
                var branchNode = node as BranchNode;
                var trueNode = branchNode.GetPortNode("True");
                var falseNode = branchNode.GetPortNode("False");

                if (trueNode && this.IsAsyncNode(trueNode, cache)) {
                    cache.Add(node, true);
                    return true;
                }
                else if (falseNode && this.IsAsyncNode(falseNode, cache)) {
                    cache.Add(node, true);
                    return true;
                }
            }

            /*
            foreach (var port in node.Inputs) {
                if (port.fieldName == "In") {
                    continue;
                }

                var n = node.GetPortNode(port.fieldName);
                
                if (n && n != node && this.IsAsyncNode(n)) {
                    return true;
                }
            }
            */

            if (node.NextNode && this.IsAsyncNode(node.NextNode, cache)) {
                cache.Add(node, true);
                return true;
            }

            cache.Add(node, false);
            return false;
        }
    }
}