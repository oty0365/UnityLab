using System;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateGraph.Editor
{
    public class SGNodeView : Node
    {
        public event Action<string,string,NodeType> OnNameUpdated;
        public event Action<string, ScriptableObject,NodeType> OnStateLogicUpdated;
        public event Action<string,string,NodeType> OnPortUpdated;
        public string NodeName{get;protected set;}
        public NodeType NodeType{get;protected set;}
        public string NodeID { get; protected set; }
        public string PortID {get; protected set;}
        protected SGNodeView(NodeData data)
        {
            NodeID = data.nodeID;
            NodeName = data.nodeName;
            NodeType = data.nodeType;
            title = data.nodeName;
            var titleLabel = titleContainer.Q<Label>("title-label");
            if (titleLabel != null) titleLabel.style.display = DisplayStyle.None;

            TextField nameField = new TextField { value = title };
            nameField.RegisterValueChangedCallback(evt =>
            {
                title = evt.newValue; 
                NodeName = evt.newValue; 
                OnNameUpdated?.Invoke(NodeID,NodeName,NodeType);
            });
            nameField.style.flexGrow = 1;
            nameField.style.fontSize = 14;
            nameField.style.unityFontStyleAndWeight = FontStyle.Bold;
            RegisterCallback<ContextualMenuPopulateEvent>(OnBuildContextualMenu);
            titleContainer.Insert(0, nameField);
        }

        private void CreateLogicScript()
        {
            var fileName = $"{title.Replace(" ", "")}State";
            var path = $"Assets/Scripts/StateLogic/{fileName}.cs";
        
            if (!Directory.Exists("Assets/Scripts/StateLogic"))
                Directory.CreateDirectory("Assets/Scripts/StateLogic");
        
            string template = $@"
using UnityEngine;
using System.Collections;

public class {fileName}SO : ScriptableObject, IState
{{
    private GameObject _actor;

    public bool OnStateCheck(GameObject actor)
    {{
        return true;
    }}

    public void OnStateEnter(GameObject actor)
    {{
    }}

    public void OnStateExit()
    {{
    }}
    public IEnumerator OnStateUpdate()
    {{
        yield break;
    }}
}}
";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, template);
                AssetDatabase.Refresh();
                Debug.Log($"스크립트 생성 완료: {path}");
            }
            else
            {
                Debug.LogWarning("이미 같은 이름의 스크립트가 존재합니다.");
            }
        }
        protected void Refresh()
        {
            RefreshExpandedState();
            RefreshPorts();
        }
        protected void AddScriptField(ScriptableObject scriptableObject)
        {
            var objectField = new ObjectField
            {
                objectType = typeof(ScriptableObject),
                value = scriptableObject
            };
            objectField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is IState || evt.newValue == null)
                {
                    OnStateLogicUpdated?.Invoke(NodeID,(ScriptableObject)evt.newValue,NodeType);
                }
            });
            contentContainer.Add(objectField);
            Button createScriptBtn = new Button(() => CreateLogicScript()) {
                text = "Create New State Script"
            };
        
            createScriptBtn.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            createScriptBtn.style.marginTop = 10;
    
            mainContainer.Add(createScriptBtn);
        }

        protected void AddPortField(string portID)
        {
            PortID = portID;
            var idField = new TextField { value = PortID };
            idField.RegisterValueChangedCallback(evt =>
            {
                PortID = evt.newValue;
                OnPortUpdated?.Invoke(NodeID,PortID,NodeType);
            });
            idField.style.maxWidth = 150; 
            
            var inputElement = idField.Q("unity-text-input");
            inputElement.style.overflow = Overflow.Hidden;
            
            idField.style.flexGrow = 1;
            idField.style.fontSize = 10;
            idField.style.unityFontStyleAndWeight = FontStyle.Bold;

            contentContainer.Add(idField);
        }
        protected virtual void OnBuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Utils/Copy ID", (a) => {
                EditorGUIUtility.systemCopyBuffer = NodeID;
                Debug.Log($"ID 복사됨: {NodeID}");
            });
        }
    }
}
