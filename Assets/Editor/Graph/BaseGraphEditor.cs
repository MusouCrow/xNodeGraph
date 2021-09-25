using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Game.Graph;

namespace GEditor.Graph {
    [CustomNodeGraphEditor(typeof(BaseGraph))]
    public class BaseGraphEditor : NodeGraphEditor {
        private Vector2 clickPos;
        private GenericSelector<Type> typeSelector;

        public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, XNode.NodePort.IO direction = XNode.NodePort.IO.Input) {
            this.FlushSelector();
            this.clickPos = Event.current.mousePosition;
            this.typeSelector.ShowInPopup(500, 300);
        }
        
        private void FlushSelector() {
            var types = this.GetNodeTypes();
            this.typeSelector = new GenericSelector<Type>("选择创建节点", false, types);
            this.typeSelector.EnableSingleClickToSelect();
            this.typeSelector.SelectionChanged += this.AddNode;
        }

        private void AddNode(IEnumerable<Type> types) {
            var type = types.FirstOrDefault();

            if (type != null) {
                this.AddNode(type);
            }
        }

        private void AddNode(Type type) {
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(this.clickPos);
            
            var node = this.CreateNode(type, pos);
            NodeEditorWindow.current.AutoConnect(node);
        }

        private IEnumerable<GenericSelectorItem<Type>> GetNodeTypes() {
            var types = NodeEditorReflection.nodeTypes;
            var tree = new List<GenericSelectorItem<Type>>();

            foreach (var t in types) {
                var name = this.GetNodeName(t);

                if (name == "") {
                    continue;
                }

                var item = new GenericSelectorItem<Type>(name, t);
                tree.Add(item);
            }
            
            tree.Sort(this.Sort);

            return tree;
        }

        private int Sort(GenericSelectorItem<Type> a, GenericSelectorItem<Type> b) {
            bool aHas = a.Name.Contains("/");
            bool bHas = b.Name.Contains("/");

            if (aHas && bHas) {
                return a.Name.Length > b.Name.Length ? 1 : -1;
            }
            else if (aHas) {
                return -1;
            }
            else if (bHas) {
                return 1;
            }

            return a.Name.Length > b.Name.Length ? 1 : -1;
        }

        private string GetNodeName(Type type) {
            var name = type.Name;
            var pos = name.LastIndexOf("Node");
            name = name.Substring(0, pos);

            XNode.Node.CreateNodeMenuAttribute attrib;
            NodeEditorUtilities.GetAttrib(type, out attrib);

            if (NodeEditorUtilities.GetAttrib(type, out attrib)) {
                var title = attrib.menuName;
                var pos2 = title.LastIndexOf("-");

                if (pos2 >= 0) {
                    title = title.Substring(0, pos2) + "/" + title;
                }

                return title + " (" + name + ")";
            }

            return "";
        }
    }
}