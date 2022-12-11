using System.Collections.Generic;
using DS.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Windows
{
    using Enumerations;
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DSGraphView graphView;
        private Texture2D identationIcon;
        public void Initialize(DSGraphView dsGraphView)
        {
            graphView = dsGraphView;
            identationIcon = new Texture2D(1, 1);
            identationIcon.SetPixel(0,0, Color.clear);
            identationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"),1),
                new SearchTreeEntry(new GUIContent("Single Choice", identationIcon))
                {
                    level = 2,
                    userData =  DSDialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", identationIcon))
                {
                    level = 2,
                    userData =  DSDialogueType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", identationIcon))
                {
                    level = 2,
                    userData =  new Group()
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            
            switch (searchTreeEntry.userData)
            {
                case DSDialogueType.SingleChoice:
                {
                    DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode) graphView.CreateNode(DSDialogueType.SingleChoice, localMousePosition);
                    graphView.AddElement(singleChoiceNode);
                    return true;
                }
                case DSDialogueType.MultipleChoice:
                {
                    DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode) graphView.CreateNode(DSDialogueType.MultipleChoice, localMousePosition);
                    graphView.AddElement(multipleChoiceNode);
                    return true;
                }
                case Group _:
                {
                    Group group = graphView.CreateGroup("Dialogue Group", localMousePosition);
                    graphView.AddElement(group);
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }
    }
}
