using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats=FindFirstObjectByType<Player_Stats>();
        statToolTipText=GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.Strength:
                return "每一点力量增加一点物理伤害" +
                    "\n每俩点增加一点暴击伤害";
            case StatType.Agility:
                return "每一点敏捷增加一点暴击率" +
                    "\n每俩点敏捷增加一点闪避值";
            case StatType.Intelligence:
                return "每一点智力增加一点元素伤害" +
                    "\n每俩点智力增加一点元素抗性";
            case StatType.Vitality:
                return "每一点活力增加一点防御值和五点生命值";

            case StatType.AttackSpeed:
                return "攻击速度";
            case StatType.BasicalPhysicalDamage:
                return "物理伤害";
            case StatType.ChillDamage:
                return "冰冻伤害";
            case StatType.FireDamage:
                return "燃烧伤害";
            case StatType.LightningDamage:
                return "电击伤害";
            case StatType.CritChance:
                return "暴击率";
            case StatType.CritPower:
                return "暴击伤害";
            case StatType.ArmorReduction:
                return "穿透值";

            case StatType.PhysicalScale:
                return "物理伤害因子";
            case StatType.ChillScale:
                return "冰冻伤害因子";
            case StatType.FireScale:
                return "燃烧伤害因子";
            case StatType.LightningScale:
                return "电击伤害因子";

            case StatType.Armor:
                return "防御值,最大值为85%";
            case StatType.Evasion:
                return "闪避值";
            case StatType.IceResistance:
                return "冰冻抗性,最大值为75";
            case StatType.FireResistance:
                return "燃烧抗性,最大值为75";
            case StatType.LightningResistance:
                return "电击抗性,最大值为75";

            case StatType.MaxHealth:
                return "最大生命值";
            case StatType.HealthRegen:
                return "每秒回复的生命值";

            default:return "null";

        }

    }
}
