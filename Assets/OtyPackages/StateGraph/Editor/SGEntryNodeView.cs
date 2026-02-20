using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    public class SGEntryNodeView : SGNodeView
    {
    
        public SGEntryNodeView(string nodeName,string nodeID,ScriptableObject scriptableObject) : base(nodeName,nodeID)
        {
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);
            titleContainer.style.backgroundColor = Color.darkOliveGreen;
            AddScriptField(scriptableObject);
            Refresh();
        }
    }
}