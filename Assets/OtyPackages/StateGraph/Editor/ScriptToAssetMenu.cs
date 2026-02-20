using UnityEditor;
using UnityEngine;
using System.IO;

public static class ScriptToAssetMenu
{
    [MenuItem("Assets/Create/SGTools/Generate SO from this Script", false, 1)]
    private static void CreateSOFromSelectedScript()
    {
        Object selectedObject = Selection.activeObject;
        if (!(selectedObject is MonoScript monoScript))
        {
            Debug.LogError("C# 스크립트를 선택해주세요!");
            return;
        }
        
        System.Type scriptType = monoScript.GetClass();
        
        if (scriptType == null || !typeof(ScriptableObject).IsAssignableFrom(scriptType))
        {
            Debug.LogError("이 스크립트는 ScriptableObject를 상속받지 않았습니다!");
            return;
        }
        
        ScriptableObject asset = ScriptableObject.CreateInstance(scriptType);
        
        string scriptPath = AssetDatabase.GetAssetPath(monoScript);
        string directory = Path.GetDirectoryName(scriptPath);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{directory}/{scriptType.Name}.asset");
        
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        
        Debug.Log($"[성공] {assetPath} 에셋이 생성되었습니다!");
    }
    
    [MenuItem("Assets/Create/SGTools/Generate SO from this Script", true)]
    private static bool ValidateCreateSOFromSelectedScript()
    {
        return Selection.activeObject is MonoScript;
    }
}