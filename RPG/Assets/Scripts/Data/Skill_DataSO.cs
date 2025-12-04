using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("技能介绍")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockedByDefualt;
    public SkillType skillType;
    public UpgradeData upgradeData;
    public SkillModifier[] skillModifier;

}
[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageData damageData;//该技能或武器拥有的伤害加成
}
[Serializable]
public class SkillModifier
{
    public StatType statType;
    public float value;
}
