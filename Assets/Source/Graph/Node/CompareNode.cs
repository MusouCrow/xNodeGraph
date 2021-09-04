using System.Threading.Tasks;

namespace Game.Graph {
    public class CompareNode : BaseNode {
        public enum Method {
            Equal,
            Less,
            Greater,
            LessThan,
            GreaterThan
        }

        [Input(connectionType = ConnectionType.Override)]
        public Float a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Float b;
        private BaseNode bNode;

        public Method method;

        [Output]
        public Bool ret;

        protected override void Init() {
            base.Init();
            this.aNode = this.GetPortNode("a");
            this.bNode = this.GetPortNode("b");
        }

        public override object Run(Runtime runtime) {
            var a = this.GetValue<Float>(this.a, this.aNode, runtime);
            var b = this.GetValue<Float>(this.b, this.bNode, runtime);
            this.ret.value = this.Compare(a.value, b.value);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var a = await this.GetValueAsync<Float>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<Float>(this.b, this.bNode, runtime);
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