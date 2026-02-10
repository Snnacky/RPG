using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }
    public Skill_DomainExpansion domainExpansion { get; private set; }

    public Skill_Base[] allskills { get; private set; }
    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();
        domainExpansion = GetComponentInChildren<Skill_DomainExpansion>();

        allskills = GetComponentsInChildren<Skill_Base>();
    }

    //减少技能冷却时间
    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allskills)
        {
            skill.ReduceCooldownBy(amount);
        }
    }

    public void RecoveryAllSkills()
    {
        foreach (var skill in allskills)
        {
            skill.RecoverySkillUpgrade();
        }
    }

    //通过类型寻找技能
    public Skill_Base GetSkillBaseByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return swordThrow;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.DomainExpansion: return domainExpansion;
            default:
                Debug.Log($"Skill type{type} is not implemented yet");
                return null;
        }
    }
}
