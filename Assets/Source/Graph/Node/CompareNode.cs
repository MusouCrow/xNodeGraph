using System.Threading.Tasks;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("比较数值")]
    public class CompareNode : BaseNode {
        public override string Title {
            get {
                return "比较数值";
            }
        }

        public override string Note {
            get {
                return @"比较数值，最后输出逻辑值: 
                Equal: 相等
                Less: 小于
                Greater: 大于
                LessThan: 小于等于
                GreaterThan: 大于等于";
            }
        }

        public enum Method {
            Equal,
            Less,
            Greater,
            LessThan,
            GreaterThan
        }

        [Input(connectionType = ConnectionType.Override)]
        public Number a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Number b;
        private BaseNode bNode;

        public Method method;

        [Output]
        public Bool ret;

        protected override void Init() {
            base.Init();
            this.aNode = this.GetPortNode("a");
            this.bNode = this.GetPortNode("b");
        }

        public override object Run(Runtime runtime, int id) {
            var a = this.GetValue<Number>(this.a, this.aNode, runtime);
            var b = this.GetValue<Number>(this.b, this.bNode, runtime);
            this.ret.value = this.Compare(a.value, b.value);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var a = await this.GetValueAsync<Number>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<Number>(this.b, this.bNode, runtime);
            this.ret.value = this.Compare(a.value, b.value);
            
            return this.ret;
        }

        private bool Compare(float a, float b) {
            if (this.method == Method.Equal) {
                return a == b;
            }
            else if (this.method == Method.Less) {
                return a < b;
            }
            else if (this.method == Method.Greater) {
                return a > b;
            }
            else if (this.method == Method.LessThan) {
                return a <= b;
            }
            else if (this.method == Method.GreaterThan) {
                return a >= b;
            }

            return false;
        }
    }
}