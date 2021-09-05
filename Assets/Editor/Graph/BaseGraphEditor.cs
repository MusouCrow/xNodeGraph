using System;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Game.Graph;

namespace GEditor.Graph {
    [CustomNodeGraphEditor(typeof(BaseGraph))]
    public class BaseGraphEditor : NodeGraphEditor {
        private Vector2 clickPos;

        public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, XNode.NodePort.IO direction = XNode.NodePort.IO.Input) {
            this.clickPos = Event.current.mousePosition;
            CreateNodeEditor.OpenWindow(this.AddNode);
        }

        private void AddNode(Type type) {
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(this.clickPos);
            
            var node = this.CreateNode(type, pos);
            NodeEditorWindow.current.AutoConnect(node);
        }
    }
}