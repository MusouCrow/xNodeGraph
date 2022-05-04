using System.Threading.Tasks;
using XNode;

namespace Game.Graph {
    [CreateNodeMenuAttribute("计时分支")]
    public class TimeBranchNode : FlowNode {
        public override string Title {
            get {
                return "计时分支";
            }
        }

        public override string Note {
            get {
                return "运行节点时将比较当前时间与上一次的时间差（上一次时间存储在val变量中），情况决定代码接下来的走向(Tick, Running)，最后再走Out";
            }
        }
        
        [Input(connectionType = ConnectionType.Override)]
        public Number time;
        private BaseNode timeNode;

        public string val;

        [Output]
        public Solt tick;
        private BaseNode tickNode;

        [Output]
        public Solt running;
        private BaseNode runningNode;

        protected override void Init() {
            base.Init();

            this.timeNode = this.GetPortNode("time");
            this.tickNode = this.GetPortNode("tick");
            this.runningNode = this.GetPortNode("running");
        }

        public override object Run(Runtime runtime, int id) {
            var time = this.GetValue<Number>(this.time, this.timeNode, runtime);
            
            if (!runtime.variable.ContainsKey(this.val)) {
                runtime.variable[this.val] = new Number();
            }

            var now = UnityEngine.Time.time;
            var number = runtime.variable[this.val] as Number;

            if ((now - number.value) > time.value) {
                number.value = now;
            }
            
            if (number.value == now && this.tickNode) {
                runtime.RunNode(this.tickNode, id);
            }
            else if (this.runningNode) {
                runtime.RunNode(this.runningNode, id);
            }

            return null;
        }

        public async override Task<object> RunAsync(Runtime runtime, int id) {
            var time = await this.GetValueAsync<Number>(this.time, this.timeNode, runtime);
            
            if (!runtime.variable.ContainsKey(this.val)) {
                runtime.variable[this.val] = new Number();
            }

            var now = UnityEngine.Time.time;
            var number = runtime.variable[this.val] as Number;
            
            if ((now - number.value) > time.value && this.tickNode) {
                number.value = now;
                await runtime.RunNodeAsync(this.tickNode, id);
            }
            else if (this.runningNode) {
                await runtime.RunNodeAsync(this.runningNode, id);
            }

            return null;
        }
    }
}