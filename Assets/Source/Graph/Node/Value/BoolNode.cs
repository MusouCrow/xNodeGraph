using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("类型-布尔值")]
    public class BoolNode : BaseNode {
        public override string Title {
            get {
                return "类型-布尔值";
            }
        }

        public Bool value;

        [Output]
        public Bool Ret;

        public override object Run(Runtime runtime, int id) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            await Task.CompletedTask;
            
            return this.value;
        }
    }
}