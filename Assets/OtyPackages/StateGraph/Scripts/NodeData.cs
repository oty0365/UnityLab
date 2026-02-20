using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Entry,
    State,
    Portal,
    Exit
}

[Serializable]
public class NodeData
{
    public string nodeName;
    public string nodeID;
    public NodeType nodeType;
    public Vector2 nodePosition;
    public string jumpID;
    public List<string> connections = new List<string>();
    public ScriptableObject stateLogic;
}
