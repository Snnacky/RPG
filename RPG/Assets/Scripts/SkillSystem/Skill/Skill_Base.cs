using System;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public event Action OnSkillChange;
    public Player player { get; private set; }
    public Entity_Stats playerStats { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public DamageData damageData;
    public SkillModifier[] skillModifiers;

    [Header("General details")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected SkillType skillType;//技能类型
    [SerializeField] protected SkillUpgradeType upgradeType;//技能升级类型
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        playerStats = GetComponentInParent<Entity_Stats>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed -= cooldown;//确保在出生的时候就可以使用技能
        damageData = new DamageData();
    }

    public virtual void TryUseSkill()
    {

    }

    //设置技能升级类型
    public void SetSkillUpgrade(Skill_DataSO skillData)
    {
        RemoveModifiers(playerStats, GetStringByUpgradeType(upgradeType));

        UpgradeData upgradeData = skillData.upgradeData;
        upgradeType = upgradeData.upgradeType;
        cooldown = upgradeData.cooldown;//修改升级技能后的冷却时间
        damageData = upgradeData.damageData;//攻击数据为skill_dataSO的

        ResetCoolDown();
        skillModifiers = skillData.skillModifier;
        AddModifiers(playerStats, GetStringByUpgradeType(upgradeType));
        OnSkillChange?.Invoke();

        player.ui.inGameUI.GetSkillSlot(skillType).SetupSkillSlot(skillData);
    }

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCoolDown())
        {
            Debug.Log("In CoolDown");
            return false;
        }
        return true;
    }

    public void AddModifiers(Entity_Stats playerStats, string source)
    {
        foreach (var mod in skillModifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, source);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats, string source)
    {
        foreach (var mod in skillModifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(source);
        }
    }

    protected bool Unlocked(SkillUpgradeType upgraderToCheck) => upgradeType == upgraderToCheck;
    public bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;//冷却
    public void SetSkillOnCoolDown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;//强制进入冷却
    }
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCoolDown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }

    private string GetStringByUpgradeType(SkillUpgradeType skillUpgradeType)
    {
        switch (skillUpgradeType)
        {
            case SkillUpgradeType.None: return "None";

            case SkillUpgradeType.Dash: return "Dash";
            case SkillUpgradeType.Dash_CloneOnStart: return "Dash_CloneOnStart";
            case SkillUpgradeType.Dash_CloneOnStartAndArrival: return "Dash_CloneOnStartAndArrival";
            case SkillUpgradeType.Dash_ShardOnStart: return "Dash_ShardOnStart";
            case SkillUpgradeType.Dash_ShardOnStartAndArrival: return "Dash_ShardOnStartAndArrival";

            case SkillUpgradeType.Shard: return "Shard";
            case SkillUpgradeType.Shard_MoveToEnemy: return "Shard_MoveToEnemy";
            case SkillUpgradeType.Shard_Multicast: return "Shard_Multicast";
            case SkillUpgradeType.Shard_Teleport: return "Shard_Teleport";
            case SkillUpgradeType.Shard_TeleportHpRewind: return "Shard_TeleportHpRewind";

            case SkillUpgradeType.SwordThrow: return "SwordThrow";
            case SkillUpgradeType.SwordThrow_Spin: return "SwordThrow_Spin";
            case SkillUpgradeType.SwordThrow_Pierce: return "SwordThrow_Pierce";
            case SkillUpgradeType.SwordThrow_Bounce: return "SwordThrow_Bounce";

            case SkillUpgradeType.TimeEcho: return "TimeEcho";
            case SkillUpgradeType.TimeEcho_singleAttack: return "TimeEcho_singleAttack";
            case SkillUpgradeType.TimeEcho_MultiAttack: return "TimeEcho_MultiAttack";
            case SkillUpgradeType.TimeEcho_ChanceToDuplicate: return "TimeEcho_ChanceToDuplicate";
            case SkillUpgradeType.TimeEcho_HealWisp: return "TimeEcho_HealWisp";
            case SkillUpgradeType.TimeEcho_CleanseWisp: return "TimeEcho_CleanseWisp";
            case SkillUpgradeType.TimeEcho_CooldownWisp: return "TimeEcho_CooldownWisp";

            case SkillUpgradeType.Domain_SlowingDown: return "Domain_SlowingDown";
            case SkillUpgradeType.Domain_EchoSpam: return "Domain_EchoSpam";
            case SkillUpgradeType.Domain_ShardSpam: return "Domain_ShardSpam";
            default: return "None";
        }

    }
}
