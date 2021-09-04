using System.Threading.Tasks;

namespace Game.Graph {
    public class FloatNode : BaseNode {
        public Float value;

        [Output]
        public Float Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return this.value;
        }
    }

    public class IntegerNode : BaseNode {
        public Integer value;

        [Output]
        public Integer Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return this.value;
        }
    }

    public class BoolNode : BaseNode {
        public Bool value;

        [Output]
        public Bool Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return this.value;
        }
    }

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