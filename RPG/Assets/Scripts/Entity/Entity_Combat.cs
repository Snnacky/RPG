using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;//�Ӿ�Ч��
    private Entity_Stats stats;
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("״̬Ч��")]
    [SerializeField] private float defaultDuration = 3;//��������Ч��
    [SerializeField] private float chillSlowMultiplier = .2f;//��������
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
            float elementalDamage = stats.GetElementalDamage(out ElementType elementType,.6f);//��ȡԪ���˺�
            float damage = stats.GetPhysicalDamage(out bool isCrit);//�����˺�
           
            bool targetGetHit = damegable.TakeDamage(damage, elementalDamage, elementType, transform);
            //���Ԫ�ز��ǿ�,����Ԫ��Ч��
            if (elementType != ElementType.None)
                ApplyStatusEffect(target.transform, elementType);

            if (targetGetHit)//�з��ܵ�����
            {
                vfx.UpdateOnHitColor(elementType);
                vfx.CreatOnHitVfx(target.transform,isCrit);//���ڵ������ϵ�Ч��
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

    //��ȡ��⵽��Collider
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
