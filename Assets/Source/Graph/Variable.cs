using System;

namespace Game.Graph {
    // 插槽，表示代码执行的流向
    [Serializable]
    public class Solt {}

    [Serializable]
    public class Obj {}

    [Serializable]
    public class Number {
        public float value;

        public override string ToString() {
            return this.value.ToString();
        }
    }

    [Serializable]
    public class Bool {
        public bool value;

        public override string ToString() {
            return this.value.ToString();
        }
    }
}