using System.Threading.Tasks;

namespace Game.Graph {
    public class LogicNode : BaseNode {
        public override string Title {
            get {
                return "逻辑判断";
            }
        }

        public enum Method {
            And,
            Or,
            Not
        }

        [Input(connectionType = ConnectionType.Override)]
        public Bool a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Bool b;
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
            var a = this.GetValue<Bool>(this.a, this.aNode, runtime);
            var b = this.GetValue<Bool>(this.b, this.bNode, runtime);
            this.ret.value = this.Calculate(a.value, b.value);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var a = await this.GetValueAsync<Bool>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<Bool>(this.b, this.bNode, runtime);
            this.ret.value = this.Calculate(a.value, b.value);
            
            return this.ret;
        }

        private bool Calculate(bool a, bool b) {
            if (this.method == Method.And) {
                return a && b;
            }
            else if (this.method == Method.Or) {
                return a || b;
            }
            
            return !a;
        }
    }
}