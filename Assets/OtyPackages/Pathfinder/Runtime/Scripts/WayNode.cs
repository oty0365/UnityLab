using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime
{
    public class WayNode : MonoBehaviour
    {
    

        [HideInInspector] [SerializeField] private List<WayNode> wayNodes = new List<WayNode>();
        [SerializeField] private Color currentLineColor = Color.green;
        private bool _useWeight;
        private int _weight;
    
        public bool UseWeight
        {
            get=>_useWeight;
            set => _useWeight = value;
        }

        public int Weight
        {
            get => _weight;
            set => _weight = value;
        }
        private readonly HashSet<WayNode> _wayNodesHash = new HashSet<WayNode>();

        private void Awake() 
        {
            foreach (var node in wayNodes)
            {
                if (node != null) _wayNodesHash.Add(node);
            }
        }
    
        public void AddNode(WayNode node)
        {
            if (node == null || _wayNodesHash.Contains(node)) return;
        
            wayNodes.Add(node);
            _wayNodesHash.Add(node);
        }

        public void RemoveNode(WayNode node)
        {
            if (!_wayNodesHash.Contains(node)) return;
        
            wayNodes.Remove(node);
            _wayNodesHash.Remove(node);
        }
        
        public WayNode[] GetNodes()
        {
            return wayNodes.ToArray();
        }
        public void SetLineColor(Color color)
        {
            currentLineColor = color;
        }
    
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (wayNodes == null) return;

            foreach (var neighbor in wayNodes)
            {
                if (neighbor == null) continue;
                Color lineColor = currentLineColor;
                Handles.color = lineColor;
                var dir = neighbor.transform.position - transform.position;
                var angle2D = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Handles.ConeHandleCap(0,neighbor.transform.position-(dir.normalized*0.25f),Quaternion.Euler(-angle2D,90,0),0.5f,EventType.Repaint);
                Handles.DrawBezier(
                    transform.position, 
                    neighbor.transform.position, 
                    transform.position, 
                    neighbor.transform.position, 
                    lineColor, 
                    null, 
                    5f 
                );
            }
        }

#endif
    }
}
