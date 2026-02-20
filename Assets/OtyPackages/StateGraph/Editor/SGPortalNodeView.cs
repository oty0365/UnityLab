using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateGraph.Editor
{
    public class SGPortalNodeView : SGNodeView
    {
        public SGPortalNodeView(string nodeName,string nodeID) : base(nodeName,nodeID)
        {
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = "Enter";
            inputContainer.Add(inputPort);
            titleContainer.style.backgroundColor = Color.cornflowerBlue;
            AddPortField();
            Refresh();
        }
    
    }
}