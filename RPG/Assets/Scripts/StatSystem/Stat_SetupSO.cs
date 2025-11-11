using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Defalut Stat Setup",fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - damageScale")]
    public float physicalScale = 1;
    public float chillScale = 1;
    public float fireScale = 1;
    public float lightningScale = 1;

    [Header("Defense - Physical Damage")]
    public float armor;
    public float evesion;

    [Header("Defense - Elemtental Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float inteligence;
    public float vitality;
}
