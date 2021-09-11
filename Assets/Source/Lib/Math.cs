using UnityEngine;
using System.Threading.Tasks;
using Game.Graph;

namespace Game.Lib {
    public static class Math {
        [Node("数学-相加", "a + b", false)]
        public static float Add(float a, float b) {
            return a + b;
        }

        [Node("数学-等待")]
        public async static Task<float> Wait(float v) {
            await Task.Delay(5000);

            return v;
        }

        [Node("数学-设置名称", "", true, "gameObject")]
        public static void SetName(GameObject gameObject, string named) {
            gameObject.name = named;
        }

        [Node("数学-对象", "", true)]
        public static object ObjectTT(object p) {
            return p;
        }

        [Node("数学-取名称", "", false)]
        public static string GetName(object p) {
            return p.ToString();
        }

        [Node("数学-设置整数")]
        public static void SetInt(int v) {

        }
    }
}