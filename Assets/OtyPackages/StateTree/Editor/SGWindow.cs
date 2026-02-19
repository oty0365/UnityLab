using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OtyPackages.StateTree.Editor
{
    public class STWindow : EditorWindow
    {
        private SGView _graphView;
        private string _title;
        private SGDataSO _currentData;
    
        public static void OpenWithData(SGDataSO data)
        {
            STWindow window = GetWindow<STWindow>();
            window.titleContent = new GUIContent(data.name);
            window._currentData = data; 
            window.Show();
        }
    
        private void OnEnable()
        {
            _graphView = new SGView();
            _graphView.StretchToParentSize();
            _graphView.OnViewChanged += ChangeDetected;
            _graphView.OnSave+= SaveDetected;
            rootVisualElement.Add(_graphView);
        }

        private void ChangeDetected()
        {
            STWindow window = GetWindow<STWindow>();
            window.titleContent = new GUIContent(_currentData.name+"*");
            window.Show();
        }
        private void SaveDetected()
        {
            STWindow window = GetWindow<STWindow>();
            window.titleContent = new GUIContent(_currentData.name);
            window.Show();
        }
    }
}
