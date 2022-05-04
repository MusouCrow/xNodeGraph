using System.Threading.Tasks;
using UnityEngine;

namespace Game.Graph {
    [CreateNodeMenuAttribute("连接文本")]
    public class LinkTextNode : BaseNode {
        public override string Title {
            get {
                return "连接文本";
            }
        }

        public override string Note {
            get {
                return @"将文本与数字连接生成新的文本";
            }
        }

        [Input(connectionType = ConnectionType.Override)]
        public string text;
        private BaseNode textNode;

        [Input(connectionType = ConnectionType.Override)]
        public Number number;
        private BaseNode numberNode;

        [Output]
        public string ret;

        protected override void Init() {
            base.Init();

            this.textNode = this.GetPortNode("text");
            this.numberNode = this.GetPortNode("number");
        }

        public override object Run(Runtime runtime, int id) {
            var text = this.GetValue<string>(this.text, this.textNode, runtime);
            var number = this.GetValue<Number>(this.number, this.numberNode, runtime);

            return text + number.value.ToString();
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var text = await this.GetValueAsync<string>(this.text, this.textNode, runtime);
            var number = await this.GetValueAsync<Number>(this.number, this.numberNode, runtime);

            return text + number.value.ToString();
        }
    }
}
