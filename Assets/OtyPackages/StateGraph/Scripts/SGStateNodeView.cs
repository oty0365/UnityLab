using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateTree.Editor
{
    public class SGStateNodeView : SGNodeView
    {
        public SGStateNodeView(string nodeName,string nodeID) : base(nodeName, nodeID)
        {
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = "Enter";
            inputContainer.Add(inputPort);
        
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);
            titleContainer.style.backgroundColor = titleContainer.style.backgroundColor;
            AddScriptField();
            Refresh();
        }
    }
}