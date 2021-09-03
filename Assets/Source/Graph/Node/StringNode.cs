using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game.Graph {
    public class StringNode : BaseNode {
        public string value;

        [Output]
        public string Ret;

        public override object Run(Dictionary<BaseNode, object> cache) {
            return this.value;
        }

        public async override Task<object> RunAsync(Dictionary<BaseNode, object> cache) {
            return this.value;
        }
    }
}