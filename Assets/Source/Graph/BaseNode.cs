using System.Threading.Tasks;
using XNode;

namespace Game.Graph {
    public class BaseNode : Node {
        public virtual string Title {
            get {
                return this.name;
            }
        }

        public virtual string Note {
            get {
                return "";
            }
        }

        public BaseNode NextNode {
            get;
            protected set;
        }

        protected override void Init() {
            this.NextNode = this.GetPortNode("Out");
        }

        public override object GetValue(NodePort port) {
            return null;
        }

        public virtual object Run(Runtime runtime) {
            return null;
        }

        public async virtual Task<object> RunAsync(Runtime runtime) {
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

        public T GetValue<T>(T value, BaseNode node, Runtime runtime) {
            if (node) {
                if (runtime.cache.ContainsKey(node)) {
                    return (T)runtime.cache[node];
                }

                return (T)node.Run(runtime);
            }

            return value;
        }

        public async Task<T> GetValueAsync<T>(T value, BaseNode node, Runtime runtime) {
            if (node) {
                if (runtime.cache.ContainsKey(node)) {
                    return (T)runtime.cache[node];
                }

                var v = await node.RunAsync(runtime);

                return (T)v;
            }

            return value;
        }
    }
}