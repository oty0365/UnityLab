using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class SGNodeView : Node
{
    protected SGNodeView(string nodeName)
    {
        title = nodeName;
        var titleLabel = titleContainer.Q<Label>("title-label");
        if (titleLabel != null) titleLabel.style.display = DisplayStyle.None;

        TextField nameField = new TextField { value = title };
        nameField.RegisterValueChangedCallback(evt => { title = evt.newValue; });
        nameField.style.flexGrow = 1;
        nameField.style.fontSize = 14;
        nameField.style.unityFontStyleAndWeight = FontStyle.Bold;
        titleContainer.Insert(0, nameField);

    }

    private void CreateLogicScript()
    {
        var fileName = $"{title.Replace(" ", "")}State";
        var path = $"Assets/Scripts/StateLogic/{fileName}.cs";
        
        if (!Directory.Exists("Assets/Scripts/StateLogic"))
            Directory.CreateDirectory("Assets/Scripts/StateLogic");
        
        string template = $@"
public class {fileName} : IState
{{
public void OnStateEnter(){{}}
public void OnStateExit(){{}}
}}";
        
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
    protected void AddScriptField()
    {
        var objectField = new ObjectField
        {
            objectType = typeof(IState)
        };
        contentContainer.Add(objectField);
        Button createScriptBtn = new Button(() => CreateLogicScript()) {
            text = "Create New State Script"
        };
        
        createScriptBtn.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        createScriptBtn.style.marginTop = 10;
    
        mainContainer.Add(createScriptBtn);
    }

    protected void AddPortField()
    {
        TextField nameField = new TextField("Port ID");
        mainContainer.Add(nameField);
    }
}
