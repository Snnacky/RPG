using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Stats : MonoBehaviour//统计数据
{
    public Stat maxHp;
    [Header("角色基础属性")]
    public Stat_MajorGroup major;
    [Header("攻击")]
    public Stat_OffenseGroup offense;
    [Header("防御")]
    public Stat_DefenseGroup defense;

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }
}
