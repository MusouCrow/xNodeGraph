using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("是否为空")]
    public class IsNullNode : BaseNode {
        public override string Title {
            get {
                return "是否为空";
            }
        }

        public override string Note {
            get {
                return @"判断对象是否为NULL";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public Obj value;
        private BaseNode valueNode;

        [Output]
        public Bool ret;

        protected override void Init() {
            base.Init();
            this.valueNode = this.GetPortNode("value");
        }

        public override object Run(Runtime runtime, int id) {
            var value = this.GetObject(this.valueNode, runtime);
            this.ret.value = value == null;
            
            return this.ret;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var value = await this.GetObjectAsync(this.valueNode, runtime);
            this.ret.value = value == null;
            
            return this.ret;
        }
    }
}