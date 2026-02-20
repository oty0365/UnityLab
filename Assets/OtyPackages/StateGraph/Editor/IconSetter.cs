using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OtyPackages.StateGraph.Editor
{
    [InitializeOnLoad]
    public class IconSetter
    {
        static IconSetter()
        {
            var parentType = typeof(IState);
        
            var assembly = Assembly.GetAssembly(parentType);
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (!parentType.IsAssignableFrom(type) || type == parentType) continue;
                var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/OtyPackages/StateGraph/Icons/StateIcon.png");
                if (icon != null)
                {
                    SetIcon(type, icon);
                }
            }
        }

        private static void SetIcon(Type type, Texture2D icon)
        {
            var monoScripts = MonoImporter.GetAllRuntimeMonoScripts();
            foreach (var script in monoScripts)
            {
                if (script.GetClass() != type) continue;
                EditorGUIUtility.SetIconForObject(script, icon);
                break;
            }
        }
    }
}