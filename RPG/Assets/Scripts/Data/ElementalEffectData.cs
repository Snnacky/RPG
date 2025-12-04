using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//元素效果数据
[Serializable]
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMulltiplier;

    public float burnDuration;
    public float totalBurnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;//电力累计值

    public ElementalEffectData(Entity_Stats attacker_Stats,DamageData damageData)
    {
        chillDuration = damageData.chillDuration;
        chillSlowMulltiplier = damageData.chillSlowMultiplier;

        burnDuration = damageData.burnDuration;
        totalBurnDamage = attacker_Stats.offense.fireDamage.GetValue() * damageData.burnDamageScale;//持续燃烧总伤害  只计算武器或技能携带的倍率和伤害


        shockDuration=damageData.shockDuration;
        shockDamage = attacker_Stats.offense.lightningDamage.GetValue() * damageData.shockDamageScale;//强力电击伤害    只计算武器或技能携带的倍率和伤害
        shockCharge =damageData.shockCharge;
    }
}
