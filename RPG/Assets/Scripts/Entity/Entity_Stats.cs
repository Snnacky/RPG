using UnityEngine;

public class Entity_Stats : MonoBehaviour//统计数据
{
    public Stat_SetupSO defalutStatSetup;

    public Stat_ResourceGroup resources;
    [Header("角色基础属性")]
    public Stat_MajorGroup major;
    [Header("攻击")]
    public Stat_OffenseGroup offense;
    [Header("防御")]
    public Stat_DefenseGroup defense;

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }

    //元素攻击,找出元素伤害里面最高的一类
    public float GetElementalDamage(out ElementType elementType, float scaleFactor = 1)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float shockDamage = offense.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();//智力加成
        float highestDamage = fireDamage;
        elementType = ElementType.Fire;
        if (highestDamage < iceDamage)
        {
            elementType = ElementType.Ice;
            highestDamage = iceDamage;
        }
        if (highestDamage < shockDamage)
        {
            elementType = ElementType.Lightning;
            highestDamage = shockDamage;
        }

        if (highestDamage <= 0)
        {
            elementType = ElementType.None;
            return 0;
        }

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * 0.5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (shockDamage == highestDamage) ? 0 : shockDamage * 0.5f;
        float weakerElementsDamage = bonusFire + bonusIce + bonusLightning;//其他元素加成

        float finalDamage = highestDamage + bonusElementalDamage + weakerElementsDamage;
        return finalDamage * scaleFactor;
    }

    //获取物理攻击伤害
    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalDamage = baseDamage + bonusDamage;

        //暴击率
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue();
        float critChance = baseCritChance + bonusCritChance;
        //暴击伤害
        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalDamage * critPower : totalDamage;
        return finalDamage * scaleFactor;
    }
    //元素抗性
    public float GetElementalResistance(ElementType elementType)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * 0.5f;
        switch (elementType)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
        }
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;
        return finalResistance;
    }

    //伤害减免
    public float GetArmorMitigation(float armoReduction)//护甲穿透
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;//总的护甲

        float reductionMultiplier = Mathf.Clamp(1 - armoReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;//有效的护甲值
        float mitigation = effectiveArmor / (effectiveArmor + 100);//护甲值转换成减伤值
        float mitigationCap = .85f;
        float finalMitigation = mitigation < mitigationCap ? mitigation : mitigationCap;
        return finalMitigation;
    }
    //护甲穿透
    public float GetArmorReduction()
    {
        float finalReduction = offense.armorRedction.GetValue() / 100;
        return finalReduction;
    }

    //获取生命值
    public float GetMaxHealth()
    {
        float baseHp = resources.maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }
    //获取闪避值
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;
        float totalEvaion = baseEvasion + bonusEvasion;
        float evasionCap = 85f;//能到达的最大值
        float finalEvasion = totalEvaion < evasionCap ? totalEvaion : evasionCap;
        return finalEvasion;
    }

    
    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorRedction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;

            default:
                Debug.Log($"StatType{type} not implemented yet.");
                return null;
        }

    }

    //初始化stat默认值
    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if(defalutStatSetup==null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }
        resources.maxHealth.SetBaseValue(defalutStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defalutStatSetup.healthRegen);

        major.strength.SetBaseValue(defalutStatSetup.strength);
        major.agility.SetBaseValue(defalutStatSetup.agility);
        major.intelligence.SetBaseValue(defalutStatSetup.inteligence);
        major.vitality.SetBaseValue(defalutStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defalutStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defalutStatSetup.damage);
        offense.critChance.SetBaseValue(defalutStatSetup.critChance);
        offense.critPower.SetBaseValue(defalutStatSetup.critPower);
        offense.armorRedction.SetBaseValue(defalutStatSetup.armorReduction);

        offense.iceDamage.SetBaseValue(defalutStatSetup.iceDamage);
        offense.fireDamage.SetBaseValue(defalutStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defalutStatSetup.lightningResistance);

        defense.armor.SetBaseValue(defalutStatSetup.armor);
        defense.evasion.SetBaseValue(defalutStatSetup.evesion);

        defense.iceRes.SetBaseValue(defalutStatSetup.iceResistance);
        defense.fireRes.SetBaseValue(defalutStatSetup.fireResistance);
        defense.lightningRes.SetBaseValue(defalutStatSetup.lightningResistance);
    }
}
