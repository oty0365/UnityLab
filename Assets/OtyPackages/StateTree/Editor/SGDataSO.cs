using System.Collections.Generic;
using UnityEngine;

namespace OtyPackages.StateTree.Editor
{
    [CreateAssetMenu(fileName = "StateGraph", menuName = "SG_Tools/StateGraph")]
    public class SGDataSO : ScriptableObject
    {
        [HideInInspector] public List<StateNode> nodeNames = new List<StateNode>(); 
    }
}
