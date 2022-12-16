using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Data.Save;
    using Data.Error;
    using Elements;
    using Enumerations;
    using Utilities;
    
    public class DSGraphView : GraphView
    {
        private DSSearchWindow searchWindow;
        private DSEditorWindow editorWindow;
        private MiniMap miniMap;

        private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<string, DSGroupErrorData> groups;
        private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

        private int nameErrorsAmount;

        public int NameErrorsAmount
        {
            get
            {
                return nameErrorsAmount;
            }

            set
            {
                nameErrorsAmount = value;
                if (nameErrorsAmount == 0)
                {
                    editorWindow.EnableSaving();
                }

                if (nameErrorsAmount == 1)
                {
                    editorWindow.DisableSaving();
                }
            }
        }
        
        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            editorWindow = dsEditorWindow;
            ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();
            groups = new SerializableDictionary<string, DSGroupErrorData>();
            
            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();
            
            AddStyles();
            AddMinimapStyles();
        }

        #region Overrided functions

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }
                
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        #endregion Overrided functions

        #region Manipulators 

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Add Group", 
                    actionEvent => 
                        CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
                ));
            return contextualMenuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    actionTitle, 
                    actionEvent => AddElement(
                        CreateNode("DialogueName", dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
                ));
            return contextualMenuManipulator;
        }

        #endregion Manipulators

        #region Elements creation

        public DSGroup CreateGroup(string title, Vector2 localMousePosition)
        {
            DSGroup group = new DSGroup(title, localMousePosition);
            AddGroup(group);

            AddElement(group);
            
            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is DSNode))
                {
                    continue;
                }

                DSNode node = (DSNode) selectedElement;
                group.AddElement(node);
            }
            
            return group;
        }

        public DSNode CreateNode(string nodeName, DSDialogueType dialogueType, Vector2 position, bool shoildDraw = true)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);
            node.Initialize(nodeName, this, position);
            if (shoildDraw)
            {
                node.Draw(); 
            }

            AddUngroupedNode(node);
            
            return node;
        }

        #endregion Elements creation

        #region Callbacks

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(DSGroup);
                Type edgeType = typeof(Edge);
                List<Edge> edgesToDelete = new List<Edge>();
                List<DSGroup> groupsToDelete = new List<DSGroup>();
                List<DSNode> nodesToDelete = new List<DSNode>();
                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is DSNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (selectedElement.GetType() == edgeType)
                    {
                        Edge edge = (Edge) selectedElement;
                        edgesToDelete.Add(edge);
                    }
                    
                    if (selectedElement.GetType() != groupType)
                    {
                        continue;
                    }

                    DSGroup group = (DSGroup) selectedElement;
                    groupsToDelete.Add(group);
                }

                foreach (DSGroup groupToDelete in groupsToDelete)
                {
                    List<DSNode> groupNodes = new List<DSNode>();
                    foreach (GraphElement groupElement in groupToDelete.containedElements)
                    {
                        if (!(groupElement is DSNode))
                        {
                            continue;
                        }

                        DSNode groupNode = (DSNode) groupElement;
                        groupNodes.Add(groupNode);
                    }
                    groupToDelete.RemoveElements(groupNodes);
                    RemoveGroup(groupToDelete);
                    RemoveElement(groupToDelete);
                }
                
                DeleteElements(edgesToDelete);
                
                foreach (DSNode nodeToDelete in nodesToDelete)
                {
                    if (nodeToDelete.Group != null)
                    {
                        nodeToDelete.Group.RemoveElement(nodeToDelete);
                    }
                    RemoveUngroupedNodes(nodeToDelete);
                    nodeToDelete.DisconnectAllPorts();
                    RemoveElement(nodeToDelete);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode))
                    {
                        continue;
                    }

                    DSGroup nodeGroup = (DSGroup) group;
                    DSNode node = (DSNode) element;
                    RemoveUngroupedNodes(node);
                    AddGroupedNode(node, nodeGroup);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DSNode))
                    {
                        continue;
                    }

                    DSGroup dsGroup = (DSGroup) group;
                    DSNode node = (DSNode) element;

                    RemoveGroupedNodes(node, dsGroup);
                    AddUngroupedNode(node);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DSGroup dsGroup = (DSGroup) group;
                dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                
                if (string.IsNullOrEmpty(dsGroup.title))
                {
                    if (!string.IsNullOrEmpty(dsGroup.oldTitle))
                    {
                        ++NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dsGroup.oldTitle))
                    {
                        --NameErrorsAmount;
                    }
                }
                
                RemoveGroup(dsGroup);
                
                dsGroup.oldTitle = dsGroup.title;
                AddGroup(dsGroup);
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        DSNode nextNode = (DSNode) edge.input.node;

                        DSChoiceSaveData choiceData = (DSChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;
                        DSChoiceSaveData choiceData = (DSChoiceSaveData) edge.output.userData;
                        choiceData.NodeID = "";
                    }
                }
                
                return changes;
            };
        }
        
        #endregion Callbacks
        
        #region Repeated Elements

        public void AddUngroupedNode(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            
            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }
            
            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);
            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            
            node.SetErrorStyle(errorColor);
            
            if (ungroupedNodesList.Count == 2)
            {
                ++NameErrorsAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
            
        }
        
        public void RemoveUngroupedNodes(DSNode node)
        {
            string nodeName = node.DialogueName.ToLower();
            
            List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Remove(node);
            node.ResetStyle();
            
            if (ungroupedNodesList.Count == 1)
            {
                --NameErrorsAmount;
                ungroupedNodesList[0].ResetStyle();
                return;
            }
            
            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }            
            
        }

        public void AddGroupedNode(DSNode node, DSGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = group;
            
            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
            }
            
            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                DSNodeErrorData nodeErrorData = new DSNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                
                groupedNodes[group].Add(nodeName, nodeErrorData);
                return;
            }
            
            List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes; 
            groupedNodesList.Add(node);
            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);
            
            if (groupedNodesList.Count ==2)
            {
                ++NameErrorsAmount;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }      
            
        }

        public void RemoveGroupedNodes(DSNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;
            
            List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            
            groupedNodesList.Remove(node);
            node.ResetStyle();
            
            if (groupedNodesList.Count == 1)
            {
                --NameErrorsAmount;
                groupedNodesList[0].ResetStyle();
                return;
            }
            
           if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);
                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
            
        }
        
        private void AddGroup(DSGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                DSGroupErrorData groupErrorData = new DSGroupErrorData();
                groupErrorData.Groups.Add(group);
                groups.Add(groupName, groupErrorData);
                
                return;
            }

            List<DSGroup> groupsList = groups[groupName].Groups;
            groupsList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++NameErrorsAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        
        private void RemoveGroup(DSGroup group)
        {
            string oldGroupName = group.oldTitle.ToLower();
            List<DSGroup> groupsList = groups[oldGroupName].Groups;
            groupsList.Remove(group);
            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --NameErrorsAmount;
                groupsList[0].ResetStyle();
                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        #endregion Repeated Elements
        
        #region Elements addition

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = contex => 
                SearchWindow.Open(new SearchWindowContext(contex.screenMousePosition), searchWindow);
        }
        
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyle.uss", 
                "DialogueSystem/DSNodeStyles.uss"
                );
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        
        private void AddMinimap()
        {
            miniMap = new MiniMap()
            {
                anchored = true
            };
            
            miniMap.SetPosition(new Rect(15, 50, 200, 180));
            Add(miniMap);
            miniMap.visible = false;
        }
        
        private void AddMinimapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));
            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
        }

        #endregion Elements addition

        #region Unitilies

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            
            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }
            
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();

            NameErrorsAmount = 0;
        }

        public void ToggleMinimap()
        {
            miniMap.visible = !miniMap.visible;
        }

        #endregion Unitilies
        
    }
}
