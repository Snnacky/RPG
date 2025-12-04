using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用于武器和技能的伤害添加
[Serializable]
public class DamageData 
{
    [Header("元素类型")]
    public ElementType elementType;



    [Header("冰冻")]

    public float chillDuration = 3;
    public float chillSlowMultiplier = .2f;

    [Header("灼烧")]

    public float burnDuration = 3;
    public float burnDamageScale = 1;//持续燃烧倍率

    [Header("电击")]

    public float shockDuration = 3;
    public float shockDamageScale = 1;//强力电击倍率
    public float shockCharge = .4f;//电力累计值
}
