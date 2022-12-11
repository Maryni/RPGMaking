using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = DSDialogueType.MultipleChoice;
            Choices.Add("New Choice");
            
        }

        public override void Draw()
        {
            base.Draw();
            
            //Main container
            Button addChoiceButton = DSElementUtility.CreateButton("Add choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");
                Choices.Add("New Choice");
                outputContainer.Add(choicePort);
            });
            
            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1,addChoiceButton);
            
            //Output container
            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();
            Button deleteChoiceButton = DSElementUtility.CreateButton("X");
            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choice);
            choiceTextField.AddClasses(
                "ds-node__textfield",
                "ds-node__choice-textfield",
                "ds-node__textfield__hidden"
                );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
    }
}