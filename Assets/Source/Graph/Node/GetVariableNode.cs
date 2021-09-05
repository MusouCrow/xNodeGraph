using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("获取变量")]
    public class GetVariableNode : BaseNode {
        public override string Title {
            get {
                return "获取变量";
            }
        }

        public override string Note {
            get {
                return @"根据名字获取变量";
            }
        }

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