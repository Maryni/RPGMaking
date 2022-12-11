using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    
    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text";
            
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            //Title Container
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);

            dialogueNameTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__filename-textfield",
                "ds-node__textfield__hidden"
            );
            
            titleContainer.Insert(0, dialogueNameTextField);
            
            //Input Container
            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            
            //Extensions container
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textField = DSElementUtility.CreateTextAre(Text);
            textField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield"
                );
            
            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
            
        }
    }
}
