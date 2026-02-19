using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateTree.Editor
{
    public class SGEntryNodeView : SGNodeView
    {
    
        public SGEntryNodeView(string nodeName) : base(nodeName)
        {
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);
            titleContainer.style.backgroundColor = Color.darkOliveGreen;
            AddScriptField();
            Refresh();
        }
    }
}