using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;//视觉效果
    private Entity_Stats stats;
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("状态效果")]
    [SerializeField] private float defaultDuration = 3;//冰冻持续效果
    [SerializeField] private float chillSlowMultiplier = .2f;//冰冻减速
    [SerializeField] private float electrifyChargeBuildUp = .4f;
    [Space]
    [SerializeField] private float fireScale = .8f;
    [SerializeField] private float lightningScale = 2.5f;

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
            float elementalDamage = stats.GetElementalDamage(out ElementType elementType,.6f);//获取元素伤害
            float damage = stats.GetPhysicalDamage(out bool isCrit);//物理伤害
           
            bool targetGetHit = damegable.TakeDamage(damage, elementalDamage, elementType, transform);
            //如果元素不是空,附加元素效果
            if (elementType != ElementType.None)
                ApplyStatusEffect(target.transform, elementType);

            if (targetGetHit)//敌方受到攻击
            {
                vfx.UpdateOnHitColor(elementType);
                vfx.CreatOnHitVfx(target.transform,isCrit);//打在敌人身上的效果
            }

        }
    }

    public void ApplyStatusEffect(Transform target,ElementType elementType,float scaleFactor=1)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null)
            return;
        if (elementType == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChilledEffect(defaultDuration, chillSlowMultiplier*scaleFactor);
        if(elementType==ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            scaleFactor = fireScale;
            float fireDamage = stats.offense.fireDamage.GetValue()*scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }
        if(elementType==ElementType.Lightning&& statusHandler.CanBeApplied(ElementType.Lightning))
        {
            scaleFactor = lightningScale;
            float lightningDamage=stats.offense.lightningDamage.GetValue()*scaleFactor;
            statusHandler.ApplyElectrifyEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
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
