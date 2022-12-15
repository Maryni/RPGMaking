using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Utilities;
    
    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "DialogueFileName";
        private DSGraphView graphView;
        private static TextField fileNameTextField;
        private Button saveButton;
        
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolBar();

            AddStyles();
        }

        #region Element Addition

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }
        
        private void AddToolBar()
        {
            Toolbar toolbar = new Toolbar();
            fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
            saveButton = DSElementUtility.CreateButton("Save", ()=> Save());
            Button clearButton = DSElementUtility.CreateButton("Clear", ()=> Clear());
            Button resetButton = DSElementUtility.CreateButton("Reset", ()=> Reset());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            
            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");
            
            rootVisualElement.Add(toolbar);
        }

 

        private void AddGraphView()
        {
            graphView = new DSGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        #region Toolbar actions

        private void Clear()
        {
            graphView.ClearGraph();
        }

        private void Reset()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }
        
        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name",
                    "Please ensure the file name you`ve typed in is valid",
                    "Okey");
                return;
            }
            DSIOUtility.Initialize(graphView, fileNameTextField.value);
            DSIOUtility.Save();
        }

        #endregion Toolbar actions

        #endregion Element Addition

        #region Utility functions

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }
        
        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }

        #endregion Utility functions
    }
}
