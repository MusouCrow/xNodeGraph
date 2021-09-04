using System.Threading.Tasks;

namespace Game.Graph {
    public class StringNode : BaseNode {
        public string value;

        [Output]
        public string Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return this.value;
        }
    }
}