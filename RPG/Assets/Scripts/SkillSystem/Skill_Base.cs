using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player player { get; private set; }
    public Player_SkillManager skillManager { get; private set; }   
    public DamageScaleData damageScaleData {  get; private set; }

    [Header("General details")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected SkillType skillType;//技能类型
    [SerializeField] protected SkillUpgradeType upgradeType;//技能升级类型
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player=GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed -= cooldown;//确保在出生的时候就可以使用技能
        damageScaleData = new DamageScaleData();
    }

    public virtual void  TryUseSkill()
    {

    }

    //设置技能升级类型
    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;//修改升级技能后的冷却时间
        damageScaleData = upgrade.damageScale;
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType==SkillUpgradeType.None)
            return false;

        if (OnCoolDown())
        {
            Debug.Log("In CoolDown");
            return false;
        }
        return true;
    }
    protected bool Unlocked(SkillUpgradeType upgraderToCheck) => upgradeType == upgraderToCheck;
    public bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;//冷却
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;//强制进入冷却
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCoolDown()=>lastTimeUsed=Time.time;

}
