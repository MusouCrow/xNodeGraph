using System;
using UnityEngine;

namespace Game.Graph {
    // 插槽，表示代码执行的流向
    [Serializable]
    public class Solt {}

    public class Variables<T> {
        public T value;

        public override string ToString() {
            return this.value.ToString();
        }
    } 

    [Serializable]
    public class Obj : Variables<object> {}

    [Serializable]
    public class Number : Variables<float> {}

    [Serializable]
    public class Bool : Variables<bool> {}

    [Serializable]
    public class Vec3 : Variables<Vector3> {}

    [Serializable]
    public class Col : Variables<Color> {}

    [Serializable]
    public class Quat : Variables<Quaternion> {}
}