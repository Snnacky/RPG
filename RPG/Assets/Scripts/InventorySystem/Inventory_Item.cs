using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Inventory_Item 
{
    private string itemID;
    public ItemDataSO itemData;
    public int stackSize = 1;
    public ItemModifier[] modifiers { get;private set; }

    public ItemEffectDataSO itemEffectData;
    public int buyPrice { get; private set; }
    public float sellPrice { get; private set; }
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffectData = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * .35f;
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

    public string GetItemInfo()
    {
        if (itemData.itemType == ItemType.Material)
            return "用于制作";
        if (itemData.itemType == ItemType.Counsumable)
            return itemData.itemEffect.effectDescription;

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            stringBuilder.AppendLine("+" + modValue + " " + modType);
        }

        if (itemEffectData != null)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Unique effect:");
            stringBuilder.AppendLine(itemEffectData.effectDescription);
        }

        return stringBuilder.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligencee";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.BasicalPhysicalDamage: return "Basical Physical Damage";
            case StatType.CritChance: return "Crit Chance";
            case StatType.CritPower: return "Crit Power";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.PhysicalScale: return "Physical Scale";
            case StatType.ChillScale: return "Chill Scale";
            case StatType.FireScale: return "Fire Scale";
            case StatType.LightningScale: return "Lightning Scale";

            case StatType.Armor: return "Armmor";
            case StatType.Evasion: return "Evasion";

            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";

            case StatType.ChillDamage: return "Chill Damage";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            default: return "Unknown Stat";
        }
    }

    private bool IsPercentageStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            //case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default: return false;
        }
    }

}
