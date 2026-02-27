using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateGraph.Editor
{
    public class STWindow : EditorWindow
    {
        private SGView _graphView;
        private SGDataSO _currentData;
        private Toolbar _toolbar;
    
        public static void OpenWithData(SGDataSO data)
        {
            STWindow window = GetWindow<STWindow>();
            window.titleContent = new GUIContent(data.name);
            window._currentData = data; 
            window.CreateToolbar();
            window.LoadDataToView();
            window.Show();
            
        }
    
        private void OnEnable()
        {
            _graphView = new SGView();
            _graphView.StretchToParentSize();
            _graphView.OnViewChanged += ChangeDetected;
            _graphView.OnSave += SaveDetected;
            
            rootVisualElement.Add(_graphView);
            
            if (_currentData != null)
            {
                Debug.Log(_currentData);
                LoadDataToView();
            }
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            _toolbar = toolbar;
            toolbar.style.height = 30;
            toolbar.style.paddingLeft = 10;
            toolbar.style.alignItems = Align.Center;
            toolbar.style.backgroundColor = new Color(0.12f, 0.12f, 0.12f);
            toolbar.style.borderBottomWidth = 0;

            Button saveButton = new Button(() => SaveDetected())
            {
                text = "💾 Save Asset"
            };
            
            Color graphBgColor = new Color(0.12f, 0.12f, 0.12f);
            
            saveButton.style.width = 110;
            saveButton.style.height = 24;
            saveButton.style.backgroundColor = graphBgColor; 
            saveButton.style.color = new Color(0.7f, 0.7f, 0.7f); 
            saveButton.style.unityFontStyleAndWeight = FontStyle.Bold; 
            
            saveButton.style.borderTopLeftRadius = 4;
            saveButton.style.borderTopRightRadius = 4;
            saveButton.style.borderBottomLeftRadius = 4;
            saveButton.style.borderBottomRightRadius = 4;
            
            saveButton.style.borderLeftWidth = 1;
            saveButton.style.borderRightWidth = 1;
            saveButton.style.borderTopWidth = 1;
            saveButton.style.borderBottomWidth = 1;
            saveButton.style.borderLeftColor = new Color(0.2f, 0.2f, 0.2f);
            saveButton.style.borderRightColor = new Color(0.2f, 0.2f, 0.2f);
            saveButton.style.borderTopColor = new Color(0.2f, 0.2f, 0.2f);
            saveButton.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f);
            
            saveButton.RegisterCallback<MouseEnterEvent>(evt => {
                saveButton.style.backgroundColor = new Color(0.2f, 0.4f, 0.2f);
                saveButton.style.color = Color.white;
            });
            saveButton.RegisterCallback<MouseLeaveEvent>(evt => {
                saveButton.style.backgroundColor = graphBgColor;
                saveButton.style.color = new Color(0.7f, 0.7f, 0.7f);
            });
            
            toolbar.Add(saveButton);
            rootVisualElement.Add(toolbar);
        }

        private void ChangeDetected()
        {
            if (_currentData == null) return;
            titleContent = new GUIContent(_currentData.name + "*");
            if (_toolbar == null)
            {
                CreateToolbar();
            }
        }

        public void LoadDataToView()
        {
            if (_currentData != null && _graphView != null)
            {
                _graphView.ImportDatas(_currentData.nodeDatas);
            }
            Debug.Log(_currentData.entryNode);
            if (string.IsNullOrEmpty(_currentData.entryNode.nodeID))
            {
                Debug.Log("엔트리 노드 생성");
                _graphView?.CreateNode(new NodeData
                {
                    nodeType = NodeType.Entry, nodeName = "New Entry", nodeID = Guid.NewGuid().ToString(),
                    nodePosition = new Vector2(0,0),
                    connections = new List<string>(), jumpID = null
                });
                _currentData.entryNode = _graphView?.ExportEntryNodeData();
            }
            else
            {
                _graphView?.ImportEntryNodeData(_currentData.entryNode);
            }
        }

        private void SaveDetected()
        {
            if (_currentData == null) return;
            titleContent = new GUIContent(_currentData.name);
            _currentData.nodeDatas = _graphView.ExportNodeDatasAsList();
            _currentData.entryNode = _graphView?.ExportEntryNodeData();
            EditorUtility.SetDirty(_currentData);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"[{_currentData.name}] 에셋이 성공적으로 저장되었습니다.");
        }
    }
}