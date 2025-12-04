using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot 
{
    public ItemType slotType;//装备类型
    public Inventory_Item equipedItem;//物品

    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
}
