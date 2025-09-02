using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour , IDamgable
{
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;
    private Entity_VFX entity_VFX;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats stats;

    [Header("��������Ч��")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("�ع�������Ч��")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = 0.3f;//�����𺦵���ֵ
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);
    private void Awake()
    {
        entity = GetComponent<Entity>();//Entity������Ҳ����Entity
        entity_VFX = GetComponent<Entity_VFX>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponent<Entity_Stats>();
        currentHp = stats.GetMaxHealth();
        updateHealthBar();
    }
    public virtual void TakeDamage(float damage , Transform damageDealer)
    {
        if(isDead) return;
        Vector2 knockback = CalucateKnockback(damage,damageDealer);
        ReduceHp(damage);
        entity_VFX?.PlayOnDamageVfx();
        entity?.ReciveKnockback(knockback, CalucateDuration(damage));
            
    }

    protected void ReduceHp(float damage)
    {
        currentHp-=damage;
        updateHealthBar();
        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    //������˵�Ч����С
    private Vector2 CalucateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;
        return knockback;
    }

    //�������ʱ��
    private float CalucateDuration(float damge)
    {
        return IsHeavyDamage(damge) ? heavyKnockbackDuration : knockbackDuration;
    }
    //�ж��Ƿ�������
    private bool IsHeavyDamage(float damge) => damge / stats.GetMaxHealth() > heavyDamageThreshold;



    private void updateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHp / stats.GetMaxHealth();
    }
}
