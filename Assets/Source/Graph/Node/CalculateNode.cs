using System.Threading.Tasks;

namespace Game.Graph {
    public class CalculateNode : BaseNode {
        public enum Method {
            Add,
            Minus,
            Multiy
        }

        [Input(connectionType = ConnectionType.Override)]
        public Number a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Number b;
        private BaseNode bNode;

        public Method method;

        [Output]
        public Number ret;

        protected override void Init() {
            base.Init();
            this.aNode = this.GetPortNode("a");
            this.bNode = this.GetPortNode("b");
        }

        public override object Run(Runtime runtime) {
            var a = this.GetValue<Number>(this.a, this.aNode, runtime);
            var b = this.GetValue<Number>(this.b, this.bNode, runtime);
            this.ret.value = this.Calculate(a.value, b.value);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var a = await this.GetValueAsync<Number>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<Number>(this.b, this.bNode, runtime);
            this.ret.value = this.Calculate(a.value, b.value);
            
            return this.ret;
        }

        private float Calculate(float a, float b) {
            if (this.method == Method.Add) {
                return a + b;
            }
            else if (this.method == Method.Minus) {
                return a - b;
            }
            
            return a * b;
        }
    }
}