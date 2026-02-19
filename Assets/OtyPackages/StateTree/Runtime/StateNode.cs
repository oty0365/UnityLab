using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateNode
{
    public string nodeName;
    public string nodeID;
    public Vector2 nodePosition;
    public List<string>  childNodes = new List<string>();
}
