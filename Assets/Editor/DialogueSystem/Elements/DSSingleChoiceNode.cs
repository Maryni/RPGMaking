﻿using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Windows;
    using Enumerations;
    using Utilities;
    
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            DialogueType = DSDialogueType.SingleChoice;
            
            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            
            //Output container
            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}