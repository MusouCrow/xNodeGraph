using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using XNode;

namespace Game.Graph {
    // 插槽，表示代码执行的流向
    [Serializable]
    public class Solt {}

    public class BaseNode : Node {
        public override object GetValue(NodePort port) {
            return null;
        }

        public virtual object Run(Dictionary<BaseNode, object> cache) {
            return null;
        }

        public async virtual Task<object> RunAsync(Dictionary<BaseNode, object> cache) {
            return null;
        }

        public BaseNode GetPortNode(string name) {
            var port = this.GetPort(name);

            if (port != null && port.IsConnected) {
                var node = port.GetConnection(0).node as BaseNode;

                return node;
            }

            return null;
        }

        public T GetValue<T>(T value, BaseNode node, Dictionary<BaseNode, object> cache) {
            if (node) {
                if (cache.ContainsKey(node)) {
                    return (T)cache[node];
                }

                return (T)node.Run(cache);
            }

            return value;
        }

        public async Task<T> GetValueAsync<T>(T value, BaseNode node, Dictionary<BaseNode, object> cache) {
            if (node) {
                if (cache.ContainsKey(node)) {
                    return (T)cache[node];
                }

                var v = await node.RunAsync(cache);

                return (T)v;
            }

            return value;
        }
    }
}