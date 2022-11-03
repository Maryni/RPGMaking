using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> inventory = new List<Item>();
    [SerializeField] private int countSlotsMax;
    [SerializeField] private int countCurrentSlotsUsed;
    [SerializeField] private SlotsAccess slotsAccess;
    [SerializeField] private GameObject inventoryGameObject;

    public void AddItem(Item item)
    {
        if (countCurrentSlotsUsed < countSlotsMax)
        {
            var id = slotsAccess.GetFirstClearSlotId();
            if (id != -1)
            {
                var image = slotsAccess.GetFirstImage();
                image.sprite = item.ItemInfo.Sprite;
            }
            else
            {
                Debug.LogWarning($"slotAccess return -1 as clear slot");
            }
            inventory.Add(item);
            countCurrentSlotsUsed++;
            
        }
        else
        {
            Debug.LogWarning("There is no empty slots here");
        }
        //item.gameObject.SetActive(false);
    }

    public void RemoveItem(Item item)
    {
        slotsAccess.ClearSlotById(slotsAccess.GetIdBySprite(item.ItemInfo.Sprite));
        inventory.Remove(item);
        countCurrentSlotsUsed--;
    }

    public void ChangeActiveState(bool forceDisable = false)
    {
        if (!forceDisable)
        {
            inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);
        }
        else
        {
            inventoryGameObject.SetActive(false);
        }
    }
    
}
