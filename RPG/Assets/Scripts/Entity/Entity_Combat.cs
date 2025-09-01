using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float damage;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {
        
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damgable = target.GetComponent<IDamgable>();
            if (damgable == null)
                continue;
            damgable.TakeDamage(damage, transform);
            vfx.CreatOnHitVfx(target.transform);
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
