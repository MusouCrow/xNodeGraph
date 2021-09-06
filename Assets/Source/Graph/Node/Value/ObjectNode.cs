using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("类型-资源对象")]
    public class ObjectNode : BaseNode {
        public override string Title {
            get {
                return "类型-资源对象";
            }
        }

        public Object value;

        [Output]
        public Object Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            await Task.CompletedTask;
            
            return this.value;
        }
    }
}