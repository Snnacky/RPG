using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Defalut Stat Setup",fileName = "Default Stat Setup")]
public class StatSetupDataSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - 物理")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - 法术")]
    public float chillDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;

    [Header("Offense - 伤害因子")]
    public float physicalScale = 1;
    public float chillScale = 1;
    public float fireScale = 1;
    public float lightningScale = 1;

    [Header("Defense - 物理")]
    public float armor;
    public float evesion;

    [Header("Defense - 法术")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float inteligence;
    public float vitality;
}
