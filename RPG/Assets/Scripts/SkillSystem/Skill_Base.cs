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
    [SerializeField] protected SkillType skillType;//��������
    [SerializeField] protected SkillUpgradeType upgradeType;//������������
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player=GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed -= cooldown;//ȷ���ڳ�����ʱ��Ϳ���ʹ�ü���
    }

    public virtual void  TryUseSkill()
    {

    }

    //���ü�����������
    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;//�޸��������ܺ����ȴʱ��
        damageScaleData = upgrade.damageScale;
    }

    public bool CanUseSkill()
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
    public bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;//��ȴ
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;//ǿ�ƽ�����ȴ
    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCoolDown()=>lastTimeUsed=Time.time;

}
