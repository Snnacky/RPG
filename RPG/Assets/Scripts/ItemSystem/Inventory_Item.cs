using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory_Item 
{
    private string itemID;
    public ItemDataSO itemData;
    public int stackSize = 1;
    public ItemModifier[] modifiers { get;private set; }

    public ItemEffectDataSO itemEffectData;
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffectData = itemData.itemEffect;
        modifiers = EquipmentData()?.modifiers;//增值属性
        itemID=itemData.itemName+Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemID);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemID);
        }
    }
    //添加物品效果
    public void AddItemEffect(Player player)=>itemEffectData?.Subscribe(player);
    //y移除物品效果
    public void RemoveItemEffect()=>itemEffectData?.Unsubscribe();

    private EquipmentDataSO EquipmentData()
    {
        //itemData如果是equip类型,则获取equipment
        if(itemData is EquipmentDataSO equipment)
            return equipment;
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
