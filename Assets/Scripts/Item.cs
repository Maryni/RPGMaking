using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private BaseItem baseItem;
    public BaseItem ItemInfo => baseItem;

    

}
