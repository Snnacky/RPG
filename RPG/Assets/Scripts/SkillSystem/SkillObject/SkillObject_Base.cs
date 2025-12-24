using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
   
    [Space]
    public Transform originalPlayer;//释放该技能的玩家
    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float attackCheckRadius = 1;
    [SerializeField] protected float enemyCheckRadius = 10;
    protected DamageData damageData;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats playerStats;//player身上的
    protected ElementType usedElement;//应用在SkillObject_Shard的explode
    protected bool targetGotHit;
    protected Transform lastTarget;
    
    protected virtual void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //伤害判定
    protected void DamageEnemiesIndius(Transform t,float enemyCheckRadius)
    {
        //范围内的敌人
        foreach (var defender in GetEnemiersAround(t,enemyCheckRadius))
        {
            IDamgable damgable = defender.GetComponent<IDamgable>();
            if (damgable == null) continue;
            Entity_Stats defender_Stats=defender.GetComponent<Entity_Stats>();
            if(defender_Stats==null) continue;
            AttackData attackData = playerStats.GetAttackData(damageData,defender_Stats);

            //物理伤害
            float physicalDamage = attackData.physicalDamage;
            //元素伤害
            float elemDamage = attackData.elementalDamage;

            ElementType elementType = attackData.elementType;

            usedElement = elementType;
       
            targetGotHit = damgable.TakeDamage(physicalDamage, elemDamage, elementType, originalPlayer.transform);
            //造成伤害
            if (targetGotHit)
            {
                lastTarget = defender.transform;
                Instantiate(onHitVfx, defender.transform.position, Quaternion.identity);
            }

            if(elementType!=ElementType.None)//应用元素效果
                defender.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(elementType, attackData.effectData);
        }
    }

    //获取范围内距离最近的敌人
    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance=Mathf.Infinity;
        foreach (var enemy in GetEnemiersAround(transform,enemyCheckRadius))//检测范围内的所有敌人
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

    //范围检测
    protected Collider2D[] GetEnemiersAround(Transform t,float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if(targetCheck==null)
            targetCheck = transform;
        Gizmos.DrawWireSphere(targetCheck.position, attackCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, enemyCheckRadius);
    }
}
