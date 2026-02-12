using System;
using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime.Scripts
{
    [Serializable]
    public class NodeData
    {
        public bool canGo;
        public Vector3 position;
        
        public float hCoast;
        public float gCoast;
        public float fCoast=>hCoast+gCoast;
        
        public NodeData parent;
        
        public NodeData(bool walkable, Vector3 worldPos) {
            canGo = walkable;
            position = worldPos;
        }
    }
}

