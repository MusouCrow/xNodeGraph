using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Game.Graph;

namespace GEditor.Graph {
    [CustomNodeEditor(typeof(BaseNode))]
    public class BaseNodeEditor : NodeEditor {
        public override void OnHeaderGUI() {
            var self = target as BaseNode;
            GUILayout.Label(self.Title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
    }
}