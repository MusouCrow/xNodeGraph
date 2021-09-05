using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using XNodeEditor;
using Game.Graph;

namespace GEditor.Graph {
    public class CreateNodeEditor : OdinEditorWindow {
        [ValueDropdown("GetNodeTypes", SortDropdownItems = true, DropdownHeight = 300)]
        [HideLabel]
        [Space]
        public Type nodeType;
        
        private Action<Type> OnAppend;

        public static void OpenWindow(Action<Type> OnAppend) {
            var pos = Event.current.mousePosition;
            var window = ScriptableObject.CreateInstance(typeof(CreateNodeEditor)) as CreateNodeEditor;
            var rect = new Rect(pos.x + 350, pos.y + 100, 0, 0);
            window.ShowAsDropDown(rect, new Vector2(300, 80));
            window.OnAppend = OnAppend;
        }

        private IEnumerable GetNodeTypes() {
            var types = NodeEditorReflection.nodeTypes;
            var tree = new ValueDropdownList<Type>();

            foreach (var t in types) {
                var name = this.GetNodeName(t);

                if (name == "") {
                    continue;
                }

                tree.Add(name, t);
            }

            return tree;
        }

        private string GetNodeName(Type type) {
            var name = type.Name;
            var pos = name.LastIndexOf("Node");
            name = name.Substring(0, pos);

            XNode.Node.CreateNodeMenuAttribute attrib;
            NodeEditorUtilities.GetAttrib(type, out attrib);

            if (NodeEditorUtilities.GetAttrib(type, out attrib)) {
                var title = attrib.menuName.Replace("-", "/");

                return title + " (" + name + ")";
            }

            return "";
        }

        [Button("Append", 30), PropertySpace]
        private void Append() {
            if (this.nodeType == null || this.OnAppend == null) {
                this.Close();
                return;
            }
            
            this.OnAppend(this.nodeType);
            this.Close();
        }
    }
}