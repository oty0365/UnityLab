using UnityEditor;
using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime.Scripts
{
    [CustomEditor(typeof(WayNodesLinker))]
    public class WayNodesLinkerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WayNodesLinker currentNodesLinker = (WayNodesLinker)target;
        
            if (GUILayout.Button("Add WayNode", GUILayout.Height(30)))
            {
                currentNodesLinker.InstantiateNode();
            }
        
            if (GUILayout.Button("Delete Last WayNode", GUILayout.Height(30)))
            {
                currentNodesLinker.DestroyNode(-1);
            }
        }
    }
}