using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("ººƒ‹ΩÈ…‹")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockedByDefualt;
    public SkillType skillType;
    public UpgradeData upgradeData;


}
[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScale;
}

