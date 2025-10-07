using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Ïä×Ó´úÂë
public class Object_Chest : MonoBehaviour , IDamgable
{
    private Rigidbody2D rb =>GetComponent<Rigidbody2D>();
    private Animator anim=>GetComponentInChildren<Animator>();
    private Entity_VFX fx =>GetComponent<Entity_VFX>();
    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    public bool TakeDamage(float damage,float elementalDamage,ElementType elementType, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim?.SetBool("chestOpen", true);
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-50f, 50f);
        return true;
    }

}
