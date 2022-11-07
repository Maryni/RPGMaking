using UnityEngine;

public enum ItemType
{
    None,
    QuestItem,
    EquipmentItem,
    ResourceItem,
    JunkItem
}

public enum EquipmentType
{
    None,
    Helmet,
    Body,
    Gloves,
    Boots,
    Weapon
}

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Item/Create Item", order = 52)]
public class BaseItem : ScriptableObject
{
    public ItemType ItemType;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public float Durability;
    public int Count;
    public bool IsDamageble;
}
