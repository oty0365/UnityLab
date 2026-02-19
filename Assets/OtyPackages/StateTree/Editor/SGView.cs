using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateTree.Editor
{
    public class SGView : GraphView
    {
        public event Action OnViewChanged;
        public event Action OnSave;
        public SGView()
        {
            Insert(0, new GridBackground());
            style.backgroundColor = new Color(0.12f, 0.12f, 0.12f);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            SetupContextMenu();
            graphViewChanged = OnGraphViewChanged;
            style.flexGrow = 1;
            
            RegisterCallback<KeyDownEvent>(evt =>
            {
                if (!evt.actionKey || evt.keyCode != KeyCode.S) return;
                evt.StopPropagation();
                OnSave?.Invoke();
                Debug.Log("단축키로 저장되었습니다!");
            });
        }

        private void SetupContextMenu()
        {
            var menuManipulator = new ContextualMenuManipulator(menuEvent =>
            {
                Vector2 mousePos = menuEvent.mousePosition;
                menuEvent.menu.AppendAction("Add Entry",action => CreateEntryNode("New Entry", mousePos));
                menuEvent.menu.AppendAction("Add State", action => CreateStateNode("New State", mousePos));
                menuEvent.menu.AppendAction("Add Portal", action => CreatePortalNode("New Portal", mousePos));
                menuEvent.menu.AppendAction("Add Exit", action => CreateExitNode("New Exit", mousePos));
            });
            this.AddManipulator(menuManipulator);
        }

        private void CreateStateNode(string nodeName, Vector2 position)
        {
            var node = new SGStateNodeView(nodeName);
            node.SetPosition(new Rect(position, new Vector2(150, 200)));
            AddElement(node);
            OnViewChanged?.Invoke();
        }

        private void CreateEntryNode(string nodeName, Vector2 position)
        {
            var node = new SGEntryNodeView(nodeName);
            node.SetPosition(new Rect(position, new Vector2(150, 200)));
            AddElement(node);
            OnViewChanged?.Invoke();
        }

        private void CreateExitNode(string nodeName, Vector2 position)
        {
            var node = new SGExitNodeView(nodeName);
            node.SetPosition(new Rect(position, new Vector2(150, 200)));
            AddElement(node);
            OnViewChanged?.Invoke();
        }

        private void CreatePortalNode(string nodeName, Vector2 position)
        {
            var node = new SGPortalNodeView(nodeName);
            node.SetPosition(new Rect(position, new Vector2(150, 200)));
            AddElement(node);
            OnViewChanged?.Invoke();
        }
    
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            OnViewChanged?.Invoke();
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    //Debug.Log("노드 연결됨!");
                });
            }
            return graphViewChange;
        }
    }
}