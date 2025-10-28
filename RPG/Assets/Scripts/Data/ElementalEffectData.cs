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
    public float shockCharge;

    public ElementalEffectData(Entity_Stats entityStats,DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMulltiplier = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        totalBurnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;

        shockDuration=damageScale.shockDuration;
        shockDamage=entityStats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge=damageScale.shockCharge;
    }
}
