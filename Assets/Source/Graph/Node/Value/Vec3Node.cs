using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("类型-三维向量")]
    public class Vec3Node : BaseNode {
        public override string Title {
            get {
                return "类型-三维向量";
            }
        }

        public Vec3 value;

        [Output]
        public Vec3 Ret;

        public override object Run(Runtime runtime, int id) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            await Task.CompletedTask;
            
            return this.value;
        }
    }
}