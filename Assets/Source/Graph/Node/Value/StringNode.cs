using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("类型-文本")]
    public class StringNode : BaseNode {
        public override string Title {
            get {
                return "类型-文本";
            }
        }

        public string value;

        [Output]
        public string Ret;

        public override object Run(Runtime runtime, int id) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            await Task.CompletedTask;
            
            return this.value;
        }
    }
}