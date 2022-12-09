using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogueType = DSDialogueType.SingleChoice;
            
            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            
            //Output container
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single,
                    typeof(bool));
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}