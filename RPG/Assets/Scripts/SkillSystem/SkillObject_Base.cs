using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;//Ӧ����SkillObject_Shard��explode

    //�˺��ж�
    protected void DamageEnemiesIndius(Transform t,float radius)
    {
        foreach (var target in EnemiersAround(t,radius))
        {
            IDamgable damgable = target.GetComponent<IDamgable>();
            if (damgable == null) continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);

            //�����˺�
            float physicalDamage = attackData.physicalDamage;
            //Ԫ���˺�
            float elemDamage = attackData.elementalDamage;

            ElementType element = attackData.element;

            usedElement = element;
       
            damgable.TakeDamage(physicalDamage, elemDamage, element, transform);

            if(element!=ElementType.None)//Ӧ��Ԫ��Ч��
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, attackData.effectData);
        }
    }

    //��ȡ��Χ�ھ�������ĵ���
    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance=Mathf.Infinity;
        foreach (var enemy in EnemiersAround(transform,10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance<closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
        return target;
    }

    //��Χ���
    protected Collider2D[] EnemiersAround(Transform t,float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if(targetCheck==null)
            targetCheck = transform;
        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
