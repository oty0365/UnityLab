using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    public class SGExitNodeView : SGNodeView
    {
        public SGExitNodeView(NodeData data) : base(data)
        {
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = "Enter";
            inputContainer.Add(inputPort);
            titleContainer.style.backgroundColor = Color.orangeRed;
            AddScriptField(data.stateLogic);
            Refresh();
        }
    }
}