using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    public class NumberNode : BaseNode {
        public override string Title {
            get {
                return "数值";
            }
        }

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
        public override string Title {
            get {
                return "布尔值";
            }
        }

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
        public override string Title {
            get {
                return "文本";
            }
        }

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
        public override string Title {
            get {
                return "资源对象";
            }
        }

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