using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float damage;

    public void PerformAttack()
    {
        
        foreach (var target in GetDetectedColliders())
        {
            Entity_Health targetHealth= target.GetComponent<Entity_Health>();
            targetHealth?.TakeDamage(damage, transform);
        }
    }

    //获取攻击到的Collider
    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
