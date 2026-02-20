using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    public class SGExitNodeView : SGNodeView
    {
        public SGExitNodeView(string nodeName,string nodeID,ScriptableObject scriptableObject) : base(nodeName,nodeID)
        {
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            inputPort.portName = "Enter";
            inputContainer.Add(inputPort);
            titleContainer.style.backgroundColor = Color.orangeRed;
            AddScriptField(scriptableObject);
            Refresh();
        }
    }
}