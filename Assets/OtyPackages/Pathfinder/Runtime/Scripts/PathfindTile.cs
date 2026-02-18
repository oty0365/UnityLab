using OtyPackages.Pathfinder.Runtime.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PathfindTile", menuName = "PathfindTile")]
public class PathfindTile : Tile
{
    public bool ableToGo;
    public float coast;
    
}
