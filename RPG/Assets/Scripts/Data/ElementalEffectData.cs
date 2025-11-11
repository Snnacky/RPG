using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        totalBurnDamage = damageData.burnDamage * damageData.burnDamageScale;//持续燃烧总伤害  只计算武器或技能携带的倍率和伤害
        //totalBurnDamage = attacker_Stats.offense.fireDamage.GetValue() * damageData.burnDamageScale;

        shockDuration=damageData.shockDuration;
        shockDamage=damageData.shockDamage*damageData.shockDamageScale;//强力电击伤害    只计算武器或技能携带的倍率和伤害
        //shockDamage=attacker_Stats.offense.lightningDamage.GetValue() * damageData.shockDamageScale;
        shockCharge =damageData.shockCharge;
    }
}
