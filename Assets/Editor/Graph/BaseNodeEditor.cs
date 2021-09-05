using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Sirenix.OdinInspector.Editor;
using Game.Graph;

namespace GEditor.Graph {
    [CustomNodeEditor(typeof(BaseNode))]
    public class BaseNodeEditor : NodeEditor {
        public override void OnHeaderGUI() {
            var self = target as BaseNode;
            GUILayout.Label(self.Title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }
    }

    [CustomEditor(typeof(BaseNode), true)]
    public class BaseNodeViewEditor : OdinEditor {
        public override void OnInspectorGUI() {
            var self = target as BaseNode;

            var style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            if (self.Title == self.name) {
                GUILayout.Label(self.name, style); 
            }
            else {
                GUILayout.Label(self.Title + " (" + self.name + ")", style); 
            }

            var note = self.Note;

            if (note != "") {
                style = EditorStyles.helpBox;
                EditorGUILayout.LabelField(note, style);
            }

            base.OnInspectorGUI();
        }
    }
}