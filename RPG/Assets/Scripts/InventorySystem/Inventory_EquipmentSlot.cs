using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot 
{
    public ItemType slotType;//装备类型
    public string equipedId;//唯一ID
    public Inventory_Item equipedItem;//物品


    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
    public Inventory_EquipmentSlot()
    {
        // 只在对象出生时生成一次
        equipedId = System.Guid.NewGuid().ToString();
    }
}
