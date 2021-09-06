using Game.Graph;

namespace Game.Lib {
    public static class Math {
        [Node("数学-相加", "a + b")]
        public static float Add(float a, float b) {
            return a + b;
        }

        [Node("数学-相减", "a - b")]
        public static void Sub() {
        }
    }
}