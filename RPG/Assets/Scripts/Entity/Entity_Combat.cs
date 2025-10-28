using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;//�Ӿ�Ч��
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

            float physicalDamage = attackData.physicalDamage;//��ȡ�����˺�
            
            float elementalDamage = attackData.elementalDamage;//��ȡԪ���˺�

            ElementType elementType = attackData.element;
           
            bool targetGetHit = damegable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);
            //���Ԫ�ز��ǿ�,����Ԫ��Ч��
            if(elementType!=ElementType.None)
                statusHandler?.ApplyStatusEffect(elementType, attackData.effectData);
            Debug.Log(elementType);
            if (targetGetHit)//�з��ܵ�����
                vfx.CreatOnHitVfx(target.transform,attackData.isCrit,elementType);//���ڵ������ϵ�Ч��


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
