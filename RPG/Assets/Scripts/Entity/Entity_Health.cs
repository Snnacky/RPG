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
    [Header("Ѫ������")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;

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
        entityStats = GetComponent<Entity_Stats>();
        currentHp = entityStats.GetMaxHealth();
        updateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }
    public virtual bool TakeDamage(float damage , float elementalDamage,ElementType elementType, Transform damageDealer)//����������
    {
        if (isDead) return false;
        if (AttackEvaded())
        {
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;//���׼���

        float mitigation = entityStats.GetArmorMitigation(armorReduction);//���㻤�׼���ת�����˺�����(�ٷֱ�)
        float physicalDamageTaken = damage * (1 - mitigation);//�˺����������������˺�

        float resistance = entityStats.GetElementalResistance(elementType);//Ԫ�ؿ���
        float elementalDamageTaken = elementalDamage * (1 - resistance);//�˺�����������Ԫ���˺�

        ReduceHp(physicalDamageTaken + elementalDamageTaken);//��Ѫ
        TakeKnockback(damageDealer, physicalDamageTaken);//����Ч��
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

    //�Ƿ����ܵ�
    private bool AttackEvaded()=> Random.Range(0, 100) < entityStats.GetEvasion();
    

    public void ReduceHp(float damage)
    {
        entity_VFX?.PlayOnDamageVfx();//������ɫ��ɫ
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

    //��ȡ��ǰ�����İٷֱ�
    public float GetHealthPercent() => currentHp / entityStats.GetMaxHealth();
    //���µ�ǰѪ�����ݰٷֱ�
    public void SetHealthToPercent(float percent)
    {
        currentHp = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        updateHealthBar();
    }

    //������˵�Ч����С
    private Vector2 CalucateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x = knockback.x * direction;
        return knockback;
    }

    //�ܹ�������
    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalucateKnockback(finalDamage, damageDealer);
        entity?.ReciveKnockback(knockback, CalucateDuration(finalDamage));//����Ч��
    }

    //�������ʱ��
    private float CalucateDuration(float damge)
    {
        return IsHeavyDamage(damge) ? heavyKnockbackDuration : knockbackDuration;
    }
    //�ж��Ƿ�������
    private bool IsHeavyDamage(float damge) => damge / entityStats.GetMaxHealth() > heavyDamageThreshold;

    private void updateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHp / entityStats.GetMaxHealth();
    }
}
