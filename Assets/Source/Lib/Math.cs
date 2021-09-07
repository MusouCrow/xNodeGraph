using System.Threading.Tasks;
using Game.Graph;

namespace Game.Lib {
    public static class Math {
        [Node("数学-相加", "a + b", false)]
        public static float Add(float a, float b) {
            return a + b;
        }

        public async static Task<float> Wait(float v) {
            await Task.Delay(1000);

            return v;
        }
    }
}