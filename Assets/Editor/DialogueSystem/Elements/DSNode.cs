using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Windows;
    using Enumerations;
    using Utilities;
    using Data.Save;
    
    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        protected DSGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = "DialogueName";
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialogue text";

            graphView = dsGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
            base.BuildContextualMenu(evt);
        }

        public virtual void Draw()
        {
            //Title Container
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField) callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }
                
                if (Group == null)
                {
                    graphView.RemoveUngroupedNodes(this);
                    DialogueName = target.value;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                DSGroup currentGroup = Group;
                
                graphView.RemoveGroupedNodes(this, Group);
                DialogueName = callback.newValue;
                graphView.AddGroupedNode(this, currentGroup);
            });

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
            TextField textField = DSElementUtility.CreateTextAre(Text, null, callback =>
            {
                Text = callback.newValue;
            });
            textField.AddClasses(
                "ds-node__textfield",
                "ds-node__quote-textfield"
                );
            
            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Utility functions

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }
                
                graphView.DeleteElements(port.connections);
            }
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer); 
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return inputPort.connected;
        }
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }

        #endregion Utility functions
    }
}
