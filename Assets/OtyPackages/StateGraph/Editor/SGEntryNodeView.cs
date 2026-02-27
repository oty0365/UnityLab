using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    public class SGEntryNodeView : SGNodeView
    {
        public SGEntryNodeView(NodeData data) : base(data)
        {
            capabilities &= ~Capabilities.Deletable;
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);
            titleContainer.style.backgroundColor = Color.darkOliveGreen;
            AddScriptField(data.stateLogic);
            Refresh();
        }
    }
}