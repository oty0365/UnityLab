using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime
{
    [CustomEditor(typeof(WayNode))]
    public class WayNodeEditor : Editor
    {
        private static WayNode _startNode;
        private static bool _isConnecting;
        private static SelectionType _selectionType;
        private enum SelectionType
        {
            Add,
            Remove
        }
        private ReorderableList _reorderableList;

        private void OnEnable()
        {
            var prop = serializedObject.FindProperty("wayNodes");
            _reorderableList = new ReorderableList(serializedObject, prop, true, true, true, true)
            {
                onRemoveCallback = (list) =>
                {
                    var element = prop.GetArrayElementAtIndex(list.index);
                    var nodeToRemove = element.objectReferenceValue as WayNode;
                    if (nodeToRemove != null)
                    {
                        Undo.RecordObject(target, "Remove Node Reference");
                        ((WayNode)target).RemoveNode(nodeToRemove);
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                    EditorUtility.SetDirty(target);
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = prop.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    WayNode node = element.objectReferenceValue as WayNode;
                    string label = (node != null) ? node.name : "Empty Node";
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, new GUIContent(label));
                },
                
            };
        }
        private void OnSceneGUI()
        {
            WayNode currentNode = (WayNode)target;
            Handles.BeginGUI();
            Vector2 screenPos = HandleUtility.WorldToGUIPoint(currentNode.transform.position);
        
            GUILayout.BeginArea(new Rect(screenPos.x + 20, screenPos.y - 20, 100, 50));
            if (!_isConnecting)
            {
                if (GUILayout.Button("Connect"))
                {
                    _startNode = currentNode;
                    _selectionType = SelectionType.Add;
                    _isConnecting = true;
                }
                GUILayout.Space(1f);
                if (GUILayout.Button("Disconnect"))
                {
                    _startNode = currentNode;
                    _selectionType = SelectionType.Remove;
                    _isConnecting = true;
                }
            }
            else if (_isConnecting && _startNode == currentNode)
            {
                if (GUILayout.Button("Cancel"))
                {
                    _isConnecting = false;
                }
            }
            GUILayout.EndArea();
            Handles.EndGUI();
        
            if (_isConnecting)
            {
                DrawConnectionLine();
                HandleSelection(Event.current);
            }
        }

        public override void OnInspectorGUI()
        {
            var script = (WayNode)target;
            EditorGUI.BeginChangeCheck();
            script.UseWeight = EditorGUILayout.Toggle("Use Weight", script.UseWeight);
            if (script.UseWeight)
            {
                script.Weight = EditorGUILayout.IntField("Weight Value", script.Weight);
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(script);
            }
            DrawDefaultInspector(); 
            serializedObject.Update();
            _reorderableList.DoLayoutList(); 
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawConnectionLine()
        {
            if (_startNode == null) return;
        
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var mouseWorldPos = ray.origin;
            mouseWorldPos.z = _startNode.transform.position.z;
            if (_selectionType == SelectionType.Add)
            {
                Handles.color = Color.green;
            }
            else if (_selectionType == SelectionType.Remove)
            {
                Handles.color = Color.red;
            }
            Handles.DrawLine(_startNode.transform.position, mouseWorldPos, 5f);
        
            SceneView.RepaintAll();
        }

        private void HandleSelection(Event e)
        {
            if (e.type != EventType.MouseDown || e.button != 0) return;
            GameObject picked = HandleUtility.PickGameObject(e.mousePosition, false);
            if (picked != null)
            {
                WayNode endNode = picked.GetComponent<WayNode>();
                if (endNode != null && endNode != _startNode)
                {
                    if (_selectionType == SelectionType.Add)
                    {
                        Undo.RecordObject(_startNode, "Connect Nodes");
                        _startNode.AddNode(endNode);
                    }
                    else if (_selectionType == SelectionType.Remove)
                    {
                        Undo.RecordObject(endNode, "Disconnect Nodes");
                        _startNode.RemoveNode(endNode);
                    }
                    EditorUtility.SetDirty(_startNode);
                    _isConnecting = false;
                    e.Use();
                }
            }
            else
            {
                _startNode = null;
                _isConnecting = false;
            }
        }
    }
}