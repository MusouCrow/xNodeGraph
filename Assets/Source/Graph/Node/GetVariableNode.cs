using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class GetVariableNode : BaseNode {
        public string var;

        [Output]
        public Obj ret;

        public override object Run(Runtime runtime) {
            return runtime.variable[this.var];
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return runtime.variable[this.var];
        }
    }
}