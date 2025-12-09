using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;//ItemEffect_HealOnDoingDamage
    public event Action<Collider2D> OnDoingDamage;//ItemEffect_ThunderClaw

    private Entity_VFX vfx;//视觉效果
    private Entity_Stats stats;

    public DamageData damageData;//玩家自身的伤害

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (var enemy in GetDetectedColliders())
        {
            
            IDamgable damegable = enemy.GetComponent<IDamgable>();//entity_health,object_Chest
            Entity_Stats defender_Stats = enemy.GetComponent<Entity_Stats>();
            if (damegable == null || defender_Stats == null) continue;
           
            AttackData attackData = stats.GetAttackData(damageData, defender_Stats);
            Entity_StatusHandler statusHandler = enemy.GetComponent<Entity_StatusHandler>();

            float physicalDamage = attackData.physicalDamage;//获取物理伤害

            float elementalDamage = attackData.elementalDamage;//获取元素伤害

            ElementType elementType = attackData.elementType;

            bool targetGetHit = damegable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);
            //如果元素不是空,附加元素效果
            if (elementType != ElementType.None)
                statusHandler?.ApplyStatusEffect(elementType, attackData.effectData);
            if (targetGetHit)//敌方受到攻击
            {
                OnDoingPhysicalDamage?.Invoke(physicalDamage);
                OnDoingDamage?.Invoke(enemy);
                vfx.CreatOnHitVfx(enemy.transform, attackData.isCrit, elementType);//打在敌人身上的效果
            }
            

            


        }
    }

    //获取检测到的Collider
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
