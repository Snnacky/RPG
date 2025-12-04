using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator:ScriptableObject
{
    //元素攻击
    public float GetElementalDamage(out ElementType elementType, out bool isCrit, Entity_Stats attacker_Stats, Entity_Stats defender_Stats, DamageData damageData)
    {
        isCrit = false;
        elementType = damageData.elementType;
        float baseDamage = GetElementDamageByType(elementType, attacker_Stats);
        Debug.Log(baseDamage);
        if (baseDamage == 0)
        {
            elementType = ElementType.None;
            return 0;
        }

        float bounsElementalDamage = attacker_Stats.major.intelligence.GetValue();//一点智力增加一点元素伤害
        float totalDamage=baseDamage+bounsElementalDamage;

        isCrit = Random.Range(0, 100) < CalcalateCritChance(attacker_Stats);//是否暴击
        float critDamage = isCrit ? totalDamage * CalculateCritPower(attacker_Stats) : totalDamage;//暴击计算后的伤害

        float scaleFactor = GetElementScaleByType(elementType, attacker_Stats);
        float scaleDamage = critDamage * scaleFactor;//影响因子计算后的伤害

        float elementalResistance = GetElementalResistance(defender_Stats, elementType);//元素抗性百分比\
        Debug.Log(elementalResistance);
        float finalDamage = scaleDamage * (1 - elementalResistance);//最终伤害
        Debug.Log(finalDamage);
        return finalDamage;
    }

    //物理攻击
    public float GetPhysicalDamage(out bool isCrit,Entity_Stats attacker_Stats, Entity_Stats defender_Stats)
    {
        float damage=CalculatePhysicalDamage(attacker_Stats);
        isCrit = Random.Range(0, 100) < CalcalateCritChance(attacker_Stats);//是否暴击
        
        float CritDamage = isCrit ? damage * CalculateCritPower(attacker_Stats)/100 : damage;//暴击计算后的伤害
        float scaleFactor = attacker_Stats.offense.physicalScale.GetValue();
        float scaleDamage=CritDamage * scaleFactor;//影响因子计算后的伤害

        float armorReduction = GetArmorReduction(attacker_Stats);//护甲穿透值
        float mitigation = GetArmorMitigation(defender_Stats, armorReduction);//减伤百分比
        float finalDamage = scaleDamage * (1 - mitigation);//最终伤害
        Debug.Log(finalDamage);
        return finalDamage;
    }
    //计算物理伤害
    public float CalculatePhysicalDamage(Entity_Stats attacker_Stats)
    {
        float baseDamage = attacker_Stats.offense.basicalPhysicalDamage.GetValue();
        float bounsDamage = attacker_Stats.major.strength.GetValue();//一点力量增加一点基础伤害
        float totalDamage = baseDamage + bounsDamage;
        return totalDamage;
    }

    //计算暴击率
    public float CalcalateCritChance(Entity_Stats attacker_Stats)
    {
        //暴击率
        float baseCritChance = attacker_Stats.offense.critChance.GetValue();//基础暴击率
        float bonusCritChance = attacker_Stats.major.agility.GetValue();//一点敏捷增加一点暴击率
        float critChance = baseCritChance + bonusCritChance;
        
        return critChance;
    }
    //计算暴击伤害
    public float CalculateCritPower(Entity_Stats attacker_Stats)
    {
        //暴击伤害
        float baseCritPower = attacker_Stats.offense.critPower.GetValue();//基础暴击伤害
        float bonusCritPower = attacker_Stats.major.strength.GetValue() * 0.5f;//俩点力量增加一点暴击伤害
        float critPower = (baseCritPower + bonusCritPower);
        return critPower;
    }

    //护甲伤害减免
    public float GetArmorMitigation(Entity_Stats defender_Stats, float armorReduction)//护甲穿透
    {

        float armor = CalculateArmor(defender_Stats);
        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = armor * reductionMultiplier;//有效的护甲值
        float mitigation = effectiveArmor / (effectiveArmor + 100);//护甲值转换成减伤值
        float mitigationCap = .85f;
        float finalMitigation = mitigation < mitigationCap ? mitigation : mitigationCap;
        return finalMitigation;
    }

    //计算防御值
    public float CalculateArmor(Entity_Stats defender_Stats)
    {
        float baseArmor = defender_Stats.defense.armor.GetValue();//基础护甲值
        float bonusArmor = defender_Stats.major.vitality.GetValue();//一点活力增加一点护甲值
        float totalArmor = baseArmor + bonusArmor;//总的护甲值
        return totalArmor;
    }

    //护甲穿透率
    public float GetArmorReduction(Entity_Stats attacker_Stats)
    {
        float finalReduction = attacker_Stats.offense.armorRedction.GetValue() / 100;
        return finalReduction;
    }

    //元素抗性
    public float GetElementalResistance(Entity_Stats defender_Stats,ElementType elementType)
    {
        float baseResistance = 0;
        float bonusResistance = defender_Stats.major.intelligence.GetValue() * 0.5f;//每俩点智力增加一点元素抗性
        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defender_Stats.defense.fireResistance.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defender_Stats.defense.iceResistance.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defender_Stats.defense.lightningResistance.GetValue();
                break;
        }
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;
        return finalResistance;
    }
    //获取闪避值
    public float GetEvasion(Entity_Stats defender_Stats)
    {
        float baseEvasion = defender_Stats.defense.evasion.GetValue();
        float bonusEvasion = defender_Stats.major.agility.GetValue() * 0.5f;//每俩点敏捷增加一点闪避值
        float totalEvaion = baseEvasion + bonusEvasion;
        float evasionCap = 85f;//能到达的最大值
        float finalEvasion = totalEvaion < evasionCap ? totalEvaion : evasionCap;
        return finalEvasion;
    }

    private float GetElementDamageByType(ElementType elementType , Entity_Stats attacker_Stats)
    {
        switch (elementType)
        {
            case ElementType.Ice:return attacker_Stats.offense.chillDamage.GetValue();
            case ElementType.Fire:return attacker_Stats.offense.fireScale.GetValue();
            case ElementType.Lightning:return attacker_Stats.offense.lightningDamage.GetValue();
            case ElementType.None:return 0;
            default: return 0;
        }
    }

    private float GetElementScaleByType(ElementType elementType,Entity_Stats entity_Stats)
    {
        switch (elementType)
        {
            case ElementType.Ice: return entity_Stats.offense.chillScale.GetValue();
            case ElementType.Fire: return entity_Stats.offense.fireScale.GetValue();
            case ElementType.Lightning: return entity_Stats.offense.lightningScale.GetValue();
            case ElementType.None: return 1;
            default: return 1;
        }
    }
}
