using UnityEngine;

public class Entity_Stats : MonoBehaviour//统计数据
{
    public StatSetupDataSO defalutStatSetup;

    public Stat_ResourceGroup resources;
    [Header("角色基础属性")]
    public Stat_MajorGroup major;
    [Header("攻击")]
    public Stat_OffenseGroup offense;
    [Header("防御")]
    public Stat_DefenseGroup defense;

    protected virtual void Awake()
    {

    }

    public AttackData GetAttackData(DamageData damageData,Entity_Stats defender_Stats)
    {
        return new AttackData(this, damageData, defender_Stats);
    }

    //获取生命值
    public float GetMaxHealth()
    {
        float baseHp = resources.maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        return baseHp + bonusHp;
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
            case StatType.BasicalPhysicalDamage: return offense.basicalPhysicalDamage;
            case StatType.ChillDamage:return offense.chillDamage;
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.LightningDamage: return offense.lightningDamage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorRedction;
            
            case StatType.PhysicalScale: return offense.physicalScale;
            case StatType.ChillScale: return offense.chillScale;
            case StatType.FireScale: return offense.fireScale;
            case StatType.LightningScale: return offense.lightningScale;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.IceResistance: return defense.iceResistance;
            case StatType.FireResistance: return defense.fireResistance;
            case StatType.LightningResistance: return defense.lightningResistance;

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
        offense.basicalPhysicalDamage.SetBaseValue(defalutStatSetup.damage);
        offense.critChance.SetBaseValue(defalutStatSetup.critChance);
        offense.critPower.SetBaseValue(defalutStatSetup.critPower);
        offense.armorRedction.SetBaseValue(defalutStatSetup.armorReduction);

        offense.chillDamage.SetBaseValue(defalutStatSetup.chillDamage);
        offense.fireDamage.SetBaseValue(defalutStatSetup.fireDamage);
        offense.lightningDamage.SetBaseValue(defalutStatSetup.lightningDamage);

        offense.physicalScale.SetBaseValue(defalutStatSetup.physicalScale);
        offense.chillScale.SetBaseValue(defalutStatSetup.chillScale);
        offense.fireScale.SetBaseValue(defalutStatSetup.fireScale);
        offense.lightningScale.SetBaseValue(defalutStatSetup.lightningScale);

        defense.armor.SetBaseValue(defalutStatSetup.armor);
        defense.evasion.SetBaseValue(defalutStatSetup.evesion);

        defense.iceResistance.SetBaseValue(defalutStatSetup.iceResistance);
        defense.fireResistance.SetBaseValue(defalutStatSetup.fireResistance);
        defense.lightningResistance.SetBaseValue(defalutStatSetup.lightningResistance);
    }
}
