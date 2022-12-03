using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Actual item displayed in inventory
/// </summary>
public class Item 
{
    public Item(ItemData itemData) {
        this.itemData = itemData;
    }

    public ItemData itemData;
    public int count = 1;
}
