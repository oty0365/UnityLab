using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime.Scripts
{
    public class WayNodesLinker : MonoBehaviour
    {
        [SerializeField] private WayNode nodePrefab;
        [SerializeField] private Color lineColor = Color.green;
    
        public void InstantiateNode()
        {
            var node = Instantiate(nodePrefab, transform, true);
            node.SetLineColor(lineColor);
            node.name = $"WayNode{transform.childCount}";
        }

        public void DestroyNode(int index)
        {
            if (index != -1) return;
            if (transform.childCount > 0)
            {
#if UNITY_EDITOR
                DestroyImmediate(transform.GetChild(transform.childCount-1).gameObject);
#else
            Destroy(transform.GetChild(transform.childCount-1).gameObject);
#endif
            }
        }
    
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var node in transform.GetComponentsInChildren<WayNode>())
            {
                node.SetLineColor(lineColor);
            }
        }
#endif
    
    }
}
