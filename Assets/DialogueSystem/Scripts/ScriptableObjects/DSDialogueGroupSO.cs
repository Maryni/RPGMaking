using UnityEngine;

namespace DS.ScriptableObjects
{
    public class DSDialogueGroupSO : ScriptableObject
    {
        public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}