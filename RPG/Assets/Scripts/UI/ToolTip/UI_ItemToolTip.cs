using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);
        itemName.text = itemToShow.itemData.itemName;
        itemType.text=itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return "用于制作";
        if (item.itemData.itemType == ItemType.Counsumable)
            return item.itemData.itemEffect.effectDescription;

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("");

        foreach(var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            stringBuilder.AppendLine("+" + modValue + " " + modType);
        }

        if(item.itemEffectData!=null)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Unique effect:");
            stringBuilder.AppendLine(item.itemEffectData.effectDescription);
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
            case StatType.CritChance:return "Crit Chance";
            case StatType.CritPower: return "Crit Power";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.PhysicalScale: return "Physical Scale";
            case StatType.ChillScale: return "Chill Scale";
            case StatType.FireScale: return "Fire Scale";
            case StatType.LightningScale: return "Lightning Scale";

            case StatType.Armor: return "Armmor";
            case StatType.Evasion: return "Evasion";

            case StatType.IceResistance:return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance:return "Lightning Resistance";

            case StatType.ChillDamage:return "Chill Damage";
            case StatType.FireDamage:return "Fire Damage";
            case StatType.LightningDamage:return "Lightning Damage";
            default:return "Unknown Stat";
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
            default:return false;
        }
    }
}
