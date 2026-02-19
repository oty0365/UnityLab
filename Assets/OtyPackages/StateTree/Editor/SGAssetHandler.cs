using OtyPackages.StateTree.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class SGAssetHandler
{
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var target = EditorUtility.EntityIdToObject(instanceID);
        if (target is not SGDataSO data) return false;
        STWindow.OpenWithData(data);
        return true;
    }
}