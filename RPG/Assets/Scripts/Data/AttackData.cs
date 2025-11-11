using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackData 
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType elementType;

    public ElementalEffectData effectData;//关于元素相关属性的data,持续时间....

    public DamageCalculator damageCalculator;//计算器

    public AttackData(Entity_Stats attacker_Stats,DamageData damageData,Entity_Stats defender_Stats)
    {
        damageCalculator=new DamageCalculator();
        physicalDamage = damageCalculator.GetPhysicalDamage(out isCrit, attacker_Stats, defender_Stats, damageData);
        elementalDamage = damageCalculator.GetElementalDamage(out elementType,out isCrit, attacker_Stats, defender_Stats, damageData);

        effectData=new ElementalEffectData(attacker_Stats, damageData);
    }
}
