using System.Collections.Generic;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    [CreateAssetMenu(fileName = "StateGraph", menuName = "SGTools/StateGraph")]
    public class SGDataSO : ScriptableObject
    {
        public NodeData entryNode;
        public List<NodeData> nodeDatas = new List<NodeData>(); 
    }
}
