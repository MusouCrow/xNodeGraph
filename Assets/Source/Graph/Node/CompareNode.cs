using System.Threading.Tasks;
using UnityEngine;

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
        public Integer a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Integer b;
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
            var a = this.GetValue<object>(this.a, this.aNode, runtime);
            var b = this.GetValue<object>(this.b, this.bNode, runtime);

            float va = this.ConvertValue(this.a);
            float vb = this.ConvertValue(this.b);
            this.ret.value = this.Compare(va, vb);
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime) {
            var a = await this.GetValueAsync<object>(this.a, this.aNode, runtime);
            var b = await this.GetValueAsync<object>(this.b, this.bNode, runtime);

            float va = this.ConvertValue(this.a);
            float vb = this.ConvertValue(this.b);
            this.ret.value = this.Compare(va, vb);
            
            return this.ret;
        }

        private float ConvertValue(object o) {
            float v = 0;

            if (o is Integer) {
                var vv = o as Integer;
                v = vv.value;
            }
            else if (o is Float) {
                var vv = o as Float;
                v = vv.value;
            }

            return v;
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