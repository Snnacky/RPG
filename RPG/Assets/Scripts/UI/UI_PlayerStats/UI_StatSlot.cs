using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private DamageCalculator damageCalculator;
    private Player_Stats playerStats;
    private RectTransform rect;
    private UI UI;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name="UI_Stat - "+GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }
    private void Awake()
    {
        damageCalculator = new DamageCalculator();
        rect = GetComponent<RectTransform>();
        UI=GetComponentInParent<UI>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.statToolTip.ShowToolTip(false, null);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.statToolTip.ShowToolTip(true, rect, statSlotType);
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);
        if (statToUpdate == null) return;

        float value = 0;

        switch (statSlotType)
        {
            case StatType.Strength:
                value=playerStats.major.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value=playerStats.major.vitality.GetValue();
                break;
            //攻击
            case StatType.BasicalPhysicalDamage:
                value = damageCalculator.CalculatePhysicalDamage(playerStats);
                break;
            case StatType.CritChance:
                value = damageCalculator.CalcalateCritChance(playerStats);
                break;
            case StatType.CritPower:
                value = damageCalculator.CalculateCritPower(playerStats);
                break;
            case StatType.ArmorReduction:
                value = damageCalculator.GetArmorReduction(playerStats);
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100;
                break;

            //防御
            case StatType.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen:
                value = playerStats.resources.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = damageCalculator.GetEvasion(playerStats);
                break;
            case StatType.Armor:
                value=damageCalculator.CalculateArmor(playerStats);
                break;
            //元素
            case StatType.ChillDamage:
                value=playerStats.offense.chillDamage.GetValue();  
                break;
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            //元素抗性
            case StatType.IceResistance:
                value = damageCalculator.GetElementalResistance(playerStats, ElementType.Ice) * 100;
                break;
            case StatType.FireResistance:
                value = damageCalculator.GetElementalResistance(playerStats, ElementType.Fire) * 100;
                break;
            case StatType.LightningResistance:
                value = damageCalculator.GetElementalResistance(playerStats, ElementType.Lightning) * 100;
                break;
            //影响因子
            case StatType.PhysicalScale:
                value = playerStats.offense.physicalScale.GetValue();
                break;
            case StatType.ChillScale:
                value = playerStats.offense.chillScale.GetValue();
                break;
            case StatType.FireScale:
                value = playerStats.offense.fireScale.GetValue();
                break;
            case StatType.LightningScale:
                value = playerStats.offense.lightningScale.GetValue();
                break;
        }
        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
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
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default: return false;
        }
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

            case StatType.ChillDamage:return "Chill Damage";
            case StatType.FireDamage:return "Fire Damage";
            case StatType.LightningDamage:return "Lightning Damage";

            default: return "Unknown Stat";
        }
    }

   
}
