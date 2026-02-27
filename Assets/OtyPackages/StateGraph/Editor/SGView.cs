using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace OtyPackages.StateGraph.Editor
{
    public class SGView : GraphView
    {
        public event Action OnViewChanged;
        public event Action OnSave;

        private NodeData _entryNode;
        private SGNodeView _entryNodeView;
        private Dictionary<string, NodeData> _nodeDict = new Dictionary<string, NodeData>();
        private Dictionary<string, SGNodeView> _nodeViewDict = new Dictionary<string, SGNodeView>();
        private List<NodeData> _nodeList =  new List<NodeData>();
        
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
            });
        }

        private void SetupContextMenu()
        {
            var menuManipulator = new ContextualMenuManipulator(menuEvent =>
            {
                var mousePos = menuEvent.mousePosition;
                menuEvent.menu.AppendAction("Add State", action => CreateNode(new NodeData {
                    nodeType = NodeType.State, nodeName = "New State", nodeID = Guid.NewGuid().ToString(), nodePosition = mousePos,
                    connections = new List<string>(), jumpID = null
                }));
                menuEvent.menu.AppendAction("Add Portal", action => CreateNode(new NodeData {
                    nodeType = NodeType.Portal, nodeName = "New Portal", nodeID = Guid.NewGuid().ToString(), nodePosition = mousePos,
                    connections = new List<string>(), jumpID = null
                }));
                menuEvent.menu.AppendAction("Add Exit", action => CreateNode(new NodeData {
                    nodeType = NodeType.Exit, nodeName = "New Exit", nodeID = Guid.NewGuid().ToString(), nodePosition = mousePos,
                    connections = new List<string>(), jumpID = null
                }));
            });
            this.AddManipulator(menuManipulator);
        }
        
        public void CreateNode(NodeData data)
        {
            SGNodeView nodeView;
            switch (data.nodeType)
            {
                case NodeType.Entry:
                    nodeView = new SGEntryNodeView(data);
                    break;
                case NodeType.State:
                    nodeView = new SGStateNodeView(data);
                    break;
                case NodeType.Exit:
                    nodeView = new SGExitNodeView(data);
                    break;
                case NodeType.Portal:
                    nodeView = new SGPortalNodeView(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            nodeView.OnNameUpdated += UpdateNodeName;
            nodeView.OnStateLogicUpdated += UpdateStateLogic;
            nodeView.OnPortUpdated += UpdatePortID;
            nodeView.SetPosition(new Rect(data.nodePosition, new Vector2(150, 200)));
            AddElement(nodeView);

            if (data.nodeType == NodeType.Entry)
            {
                _entryNodeView = nodeView;
                _entryNode = data;
            }
            else
            {
                _nodeList.Add(data);
            }
            _nodeDict.Add(data.nodeID, data);
            _nodeViewDict.Add(data.nodeID, nodeView);
            OnViewChanged?.Invoke();
        }
    
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                    compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            OnViewChanged?.Invoke();

            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    if (edge.output.node is SGNodeView outNode && edge.input.node is SGNodeView inNode)
                    {
                        if (_nodeDict.TryGetValue(outNode.NodeID, out var outData))
                        {
                            if (outData.connections == null) outData.connections = new List<string>();
                            if (!outData.connections.Contains(inNode.NodeID))
                                outData.connections.Add(inNode.NodeID);
                        }
                    }
                }
            }
            
            if (graphViewChange.elementsToRemove != null) 
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is Edge edge)
                    {
                        if (edge.output.node is SGNodeView outNode && edge.input.node is SGNodeView inNode)
                        {
                            if (_nodeDict.TryGetValue(outNode.NodeID, out var outData))
                                outData.connections?.Remove(inNode.NodeID);
                        }
                    }
                    else if (element is SGNodeView nodeView)
                    {
                        _nodeDict.Remove(nodeView.NodeID);
                        _nodeViewDict.Remove(nodeView.NodeID);
                    }
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (var element in graphViewChange.movedElements)
                {
                    if (element is SGNodeView nodeView && _nodeDict.TryGetValue(nodeView.NodeID, out var value))
                    {
                        value.nodePosition = element.GetPosition().position;
                    }
                }
            }

            return graphViewChange;
        }

        public List<NodeData> ExportNodeDatasAsList()
        {
            return _nodeList;
        }

        public NodeData ExportEntryNodeData()
        {
            return _entryNode;
        }

        public void ImportDatas(List<NodeData> datas)
        {
            DeleteElements(graphElements);
            _nodeDict.Clear();
            _nodeViewDict.Clear();
            
            if (datas == null) return;
            
            foreach (var data in datas)
            {
                CreateNode(data);
            }
            
            foreach (var data in datas)
            {
                if (data.connections == null) continue;

                foreach (var targetID in data.connections)
                {
                    if (_nodeViewDict.TryGetValue(data.nodeID, out var sourceView) && 
                        _nodeViewDict.TryGetValue(targetID, out var targetView))
                    {
                        LinkNodes(sourceView, targetView);
                    }
                }
            }
        }

        public void ImportEntryNodeData(NodeData heaNodeData)
        {
            CreateNode(heaNodeData);
            if (heaNodeData.connections == null) return;

            foreach (var targetID in heaNodeData.connections)
            {
                if (_nodeViewDict.TryGetValue(heaNodeData.nodeID, out var sourceView) && 
                    _nodeViewDict.TryGetValue(targetID, out var targetView))
                {
                    LinkNodes(sourceView, targetView);
                }
            }
            
        }

        private void LinkNodes(SGNodeView outNode, SGNodeView inNode)
        {
            var outputPort = outNode.outputContainer.Q<Port>(); 
            var inputPort = inNode.inputContainer.Q<Port>();

            if (outputPort == null || inputPort == null) return;
            var edge = outputPort.ConnectTo(inputPort);
            AddElement(edge);
        }

        private void UpdateNodeName(string nodeID, string nodeName, NodeType nodeType)
        {
            if (nodeType == NodeType.Entry)
            {
                _entryNode.nodeName = nodeName;
            }
            else
            {
                if (_nodeDict.TryGetValue(nodeID, out var data))
                {
                    data.nodeName = nodeName;
                }
            }
        }

        private void UpdatePortID(string nodeID, string portID,NodeType nodeType)
        {
            if (nodeType == NodeType.Entry)
            {
                _entryNode.jumpID = portID;
            }
            else
            {
                if (_nodeDict.TryGetValue(nodeID, out var data))
                {
                    data.jumpID = portID;
                }
            }
        }

        private void UpdateStateLogic(string nodeID, ScriptableObject state, NodeType nodeType)
        {
            if (nodeType == NodeType.Entry)
            {
                _entryNode.stateLogic = state;
            }
            else{
                if (_nodeDict.TryGetValue(nodeID, out var data))
                {
                    data.stateLogic = state;
                }
            }
        }
    }
}