using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;
    //物理
    [Header("攻击属性")]
    public Stat basicalPhysicalDamage;//物理伤害
    public Stat critPower;//暴击效果
    public Stat critChance;//暴击率
    public Stat armorRedction;//穿透率



    //伤害加成
    [Header("伤害加成")]
    public Stat physicalScale;
    public Stat chillScale;
    public Stat fireScale;
    public Stat lightningScale;
}
