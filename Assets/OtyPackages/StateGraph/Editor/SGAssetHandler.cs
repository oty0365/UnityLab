using OtyPackages.StateGraph.Scripts;
using UnityEditor;
using UnityEditor.Callbacks;

namespace OtyPackages.StateGraph.Editor
{
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
}