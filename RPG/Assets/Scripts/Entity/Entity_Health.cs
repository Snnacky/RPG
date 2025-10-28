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
    private Entity_Stats entityStats;
    [Header("血量再生")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;

    [Header("攻击击退效果")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("重攻击击退效果")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = 0.3f;//严重损害的阈值
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);
    private void Awake()
    {
        entity = GetComponent<Entity>();//Entity的子类也属于Entity
        entity_VFX = GetComponent<Entity_VFX>();
        healthBar = GetComponentInChildren<Slider>();
        entityStats = GetComponent<Entity_Stats>();
        currentHp = entityStats.GetMaxHealth();
        updateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }
    public virtual bool TakeDamage(float damage , float elementalDamage,ElementType elementType, Transform damageDealer)//攻击他的人
    {
        if (isDead) return false;
        if (AttackEvaded())
        {
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;//护甲减免

        float mitigation = entityStats.GetArmorMitigation(armorReduction);//计算护甲减免转换成伤害减免(百分比)
        float physicalDamageTaken = damage * (1 - mitigation);//伤害减免后的最终物理伤害

        float resistance = entityStats.GetElementalResistance(elementType);//元素抗性
        float elementalDamageTaken = elementalDamage * (1 - resistance);//伤害减免后的最终元素伤害

        ReduceHp(physicalDamageTaken + elementalDamageTaken);//扣血
        TakeKnockback(damageDealer, physicalDamageTaken);//击退效果
        return true;
    }
    
    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;
        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healthAmount)
    {
        if(isDead) return;

        float newHp = currentHp + healthAmount;
        float maxHp=entityStats.GetMaxHealth();

        currentHp=Mathf.Min(newHp, maxHp);
        updateHealthBar();
    }

    //是否闪避掉
    private bool AttackEvaded()=> Random.Range(0, 100) < entityStats.GetEvasion();
    

    public void ReduceHp(float damage)
    {
        entity_VFX?.PlayOnDamageVfx();//更换角色颜色
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

    //获取当前生命的百分比
    public float GetHealthPercent() => currentHp / entityStats.GetMaxHealth();
    //更新当前血量根据百分比
    public void SetHealthToPercent(float percent)
    {
        currentHp = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        updateHealthBar();
    }

    //计算击退的效果大小
    private Vector2 CalucateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;
        return knockback;
    }

    //受攻击击退
    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalucateKnockback(finalDamage, damageDealer);
        entity?.ReciveKnockback(knockback, CalucateDuration(finalDamage));//击退效果
    }

    //计算持续时间
    private float CalucateDuration(float damge)
    {
        return IsHeavyDamage(damge) ? heavyKnockbackDuration : knockbackDuration;
    }
    //判断是否是重损害
    private bool IsHeavyDamage(float damge) => damge / entityStats.GetMaxHealth() > heavyDamageThreshold;

    private void updateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHp / entityStats.GetMaxHealth();
    }
}
