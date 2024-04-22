using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotInfo
{
    public ItemDataObject itemData;

    public ItemSlotInfo(ItemDataObject otherItemData)
    {
        itemData = otherItemData;
    }
}
