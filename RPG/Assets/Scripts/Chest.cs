using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour , IDamgable
{
    private Rigidbody2D rb =>GetComponent<Rigidbody2D>();
    private Animator anim=>GetComponentInChildren<Animator>();
    private Entity_VFX fx =>GetComponent<Entity_VFX>();
    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    public void TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim?.SetBool("chestOpen", true);
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-50f, 50f);
    }

}
