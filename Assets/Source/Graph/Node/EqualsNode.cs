using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("比较对象")]
    public class EqualsNode : BaseNode {
        public override string Title {
            get {
                return "比较对象";
            }
        }

        public override string Note {
            get {
                return @"比较两个Unity对象是否相同";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Obj a;
        private BaseNode aNode;

        [Input(connectionType = ConnectionType.Override)]
        public Obj b;
        private BaseNode bNode;

        [Output]
        public Bool ret;

        protected override void Init() {
            base.Init();
            this.aNode = this.GetPortNode("a");
            this.bNode = this.GetPortNode("b");
        }

        public override object Run(Runtime runtime, int id) {
            var a = this.GetObject(this.aNode, runtime);
            var b = this.GetObject(this.bNode, runtime);
            
            if (a is Number && b is Number) {
                var va = a as Number;
                var vb = b as Number;
                
                this.ret.value = va.value == vb.value;
            }
            else {
                this.ret.value = a == b;
            }
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var a = await this.GetObjectAsync(this.aNode, runtime);
            var b = await this.GetObjectAsync(this.bNode, runtime);
            
            if (a is Number && b is Number) {
                var va = a as Number;
                var vb = b as Number;

                this.ret.value = va.value == vb.value;
            }
            else {
                this.ret.value = a == b;
            }
            
            return this.ret;
        }
    }
}