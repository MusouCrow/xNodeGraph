using System;

namespace Game.Graph {
    // 插槽，表示代码执行的流向
    [Serializable]
    public class Solt {}

    [Serializable]
    public class Integer {
        public int value;
    }

    [Serializable]
    public class Float {
        public float value;
    }

    [Serializable]
    public class Bool {
        public bool value;
    }
}