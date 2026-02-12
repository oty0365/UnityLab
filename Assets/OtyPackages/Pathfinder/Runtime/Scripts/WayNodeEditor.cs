using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OtyPackages.Pathfinder.Runtime.Scripts
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
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Drag & Drop Connection", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Drop to Connect", EditorStyles.miniLabel);
            Rect dropAreaAdd = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
            GUI.Box(dropAreaAdd, "Drop WayNode Here to Add Connection", EditorStyles.helpBox);
            HandleDragAndDrop(dropAreaAdd, script, true);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Drop to Disconnect", EditorStyles.miniLabel);
            Rect dropAreaRemove = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
            GUI.Box(dropAreaRemove, "Drop WayNode Here to Remove Connection", EditorStyles.helpBox);
            HandleDragAndDrop(dropAreaRemove, script, false);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(10);
            DrawDefaultInspector(); 
            
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        private void HandleDragAndDrop(Rect dropArea, WayNode currentNode, bool isAddMode)
        {
            Event evt = Event.current;
            
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;
                    
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            GameObject go = draggedObject as GameObject;
                            if (go != null)
                            {
                                WayNode draggedNode = go.GetComponent<WayNode>();
                                if (draggedNode != null && draggedNode != currentNode)
                                {
                                    if (isAddMode)
                                    {
                                        Undo.RecordObject(currentNode, "Connect Nodes via Drag");
                                        currentNode.AddNode(draggedNode);
                                        Debug.Log($"Connected: {currentNode.name} -> {draggedNode.name}");
                                    }
                                    else
                                    {
                                        Undo.RecordObject(currentNode, "Disconnect Nodes via Drag");
                                        currentNode.RemoveNode(draggedNode);
                                        Debug.Log($"Disconnected: {currentNode.name} -X- {draggedNode.name}");
                                    }
                                    EditorUtility.SetDirty(currentNode);
                                }
                            }
                        }
                    }
                    evt.Use();
                    break;
            }
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
                        Undo.RecordObject(_startNode, "Disconnect Nodes");
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