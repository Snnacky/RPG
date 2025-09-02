using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Stats : MonoBehaviour//ͳ������
{
    public Stat maxHp;
    [Header("��ɫ��������")]
    public Stat_MajorGroup major;
    [Header("����")]
    public Stat_OffenseGroup offense;
    [Header("����")]
    public Stat_DefenseGroup defense;

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }
}
