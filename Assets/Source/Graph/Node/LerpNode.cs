using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("插值运算")]
    public class LerpNode : BaseNode {
        public override string Title {
            get {
                return "插值运算";
            }
        }

        public override string Note {
            get {
                return "通过t取得a与b之间的值";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Number a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Number b;
        private BaseNode bNode;

        [Input(connectionType = ConnectionType.Override)]
        public Number t;
        private BaseNode tNode;

        [Output]
        public Number ret;

        protected override void Init() {
            base.Init();
            this.aNode = this.GetPortNode("a");
            this.bNode = this.GetPortNode("b");
            this.tNode = this.GetPortNode("t");
        }

        public override object Run(Runtime runtime, int id) {
            var a = this.GetValue<Number>(this.a, this.aNode, runtime);
            var b = this.GetValue<Number>(this.b, this.bNode, runtime);
            var t = this.GetValue<Number>(this.t, this.tNode, runtime);
            this.ret.value = Mathf.Lerp(a.value, b.value, t.value);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var a = await this.GetValueAsync<Number>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<Number>(this.b, this.bNode, runtime);
            var t = await this.GetValueAsync<Number>(this.t, this.tNode, runtime);
            this.ret.value = Mathf.Lerp(a.value, b.value, t.value);
            
            return this.ret;
        }
    }
}