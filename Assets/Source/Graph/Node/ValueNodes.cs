using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class NumberNode : BaseNode {
        public Number value;

        [Output]
        public Number Ret;

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

    public class ObjectNode : BaseNode {
        public Object value;

        [Output]
        public Object Ret;

        public override object Run(Runtime runtime) {
            return this.value;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            return this.value;
        }
    }
}