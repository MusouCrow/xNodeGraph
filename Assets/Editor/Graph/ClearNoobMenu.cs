using UnityEngine;
using UnityEditor;
using XNode;
using Game.Graph;

namespace GEditor.Graph {
    public static class ClearNoobMenu {
        [MenuItem("Tools/Graph/Clear Noob Nodes")]
        public static void ClearNoob() {
            var obj = Selection.activeObject;

            if (!obj || !(obj is NodeGraph)) {
                Debug.LogWarning("需要选择一个蓝图资源！");
                return;
            }

            var graph = obj as NodeGraph;
            bool ok = false;
            
            for (int i = graph.nodes.Count - 1; i >= 0; i--) {
                if (graph.nodes[i] == null) {
                    graph.nodes.RemoveAt(i);
                    ok = true;
                }
            }

            if (ok) {
                Debug.Log("蓝图无用节点清理完成");
            }
        }
    }
}