using System.Collections.Generic;
using UnityEngine;

public class WayNode : MonoBehaviour
{
    
    [SerializeField] private bool useWeight;
    [SerializeField] private int weight;
    [SerializeField] private List<WayNode> wayNodes = new List<WayNode>();

    public bool UseWeight
    {
        get=>useWeight;
        set => useWeight = value;
    }

    public int Weight
    {
        get => weight;
        set => weight = value;
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
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var neighbor in wayNodes)
        {
            if(neighbor == null) continue;
            bool isBiDirectional = neighbor.wayNodes.Contains(this);
            Gizmos.color = isBiDirectional ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
    #endif
}
