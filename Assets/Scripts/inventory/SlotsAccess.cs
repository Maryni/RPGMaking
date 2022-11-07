using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotsAccess : MonoBehaviour
{
    [SerializeField] private List<Image> images = new List<Image>();

    public Image GetFirstImage()
    {
       return images.FirstOrDefault(a => a.sprite == null);
    }

    public Image GetImageByIndex(int index)
    {
        if (index >= 0 && index < images.Count)
        {
            return images[index];
        }
        else
        {
            return images.FirstOrDefault(a => a.sprite == null);
        }
    }

    public int GetFirstClearSlotId()
    {
        int temp = -1;
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].sprite == null)
            {
                temp = i;
                break;
            }
        }
        return temp;
    }

    public int GetIdBySprite(Sprite sprite)
    {
        int temp = -1;
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].sprite == sprite)
            {
                temp = i;
                break;
            }
        }

        return temp;
    }

    public void ClearSlotById(int id)
    {
        images[id].sprite = null;
    }
}
