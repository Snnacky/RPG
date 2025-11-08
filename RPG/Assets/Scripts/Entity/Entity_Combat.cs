using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;//视觉效果
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;

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
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damegable = target.GetComponent<IDamgable>();//entity_health
            if (damegable == null)
                continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler=target.GetComponent<Entity_StatusHandler>();

            float physicalDamage = attackData.physicalDamage;//获取物理伤害
            
            float elementalDamage = attackData.elementalDamage;//获取元素伤害

            ElementType elementType = attackData.element;
           
            bool targetGetHit = damegable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);
            //如果元素不是空,附加元素效果
            if(elementType!=ElementType.None)
                statusHandler?.ApplyStatusEffect(elementType, attackData.effectData);
            if (targetGetHit)//敌方受到攻击
                vfx.CreatOnHitVfx(target.transform,attackData.isCrit,elementType);//打在敌人身上的效果


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
