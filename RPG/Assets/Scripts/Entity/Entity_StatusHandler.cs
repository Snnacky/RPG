using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("Shock effect details")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine ShockCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats= GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }


    //Ӧ��Ԫ�ع���Ч��
    public void ApplyStatusEffect(ElementType element,ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChilledEffect(effectData.chillDuration, effectData.chillSlowMulltiplier);
        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration, effectData.totalBurnDamage);
        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }

    //�׻�Ч��
    public void ApplyShockEffect(float duration,float damage,float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);//Ԫ�ؿ���
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;
        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);//ǿ�����
            StopShockEffect();
            return;
        }
        if(ShockCo!=null)
            StopCoroutine(ShockCo);
        ShockCo = StartCoroutine(ShockEffectCo(duration));
    }
    //ˢ�¹�������
    private void StopShockEffect()
    {
        currentEffect=ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllvfx();//ֹͣ����Э��,��Ϊ��ʼ״̬
    }
    //ǿ�����
    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);//����Ԥ����
        entityHealth.ReduceHp(damage);
    }

    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);//������ɫ
        yield return new WaitForSeconds(duration);
        //ʱ������׻�Ч��û������,����� 
        StopShockEffect();
    }

    public void ApplyBurnEffect(float duration,float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration,float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int tickPerSecond = 2;//ÿ�����մ���
        int tickCount = Mathf.RoundToInt(tickPerSecond * duration);//�����ܴ���

        float damagePerTick = totalDamage / tickCount;//ÿ�������˺�
        float tickInterval = 1f / tickPerSecond;//���
        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        currentEffect = ElementType.None;
    }

    public void ApplyChilledEffect(float duration,float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);//����
        float reduceDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCo(reduceDuration,slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float Duration,float slowMultiplier)
    {
        currentEffect = ElementType.Ice;
        entity.SlowDownEntity(Duration, slowMultiplier);//��������
        entityVfx.PlayOnStatusVfx(Duration, ElementType.Ice);//������ɫЧ��
        yield return new WaitForSeconds(Duration);
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType elementType)
    {
        if(elementType==ElementType.Lightning&&currentEffect==ElementType.Lightning)
            return true;
        return currentEffect == ElementType.None;
    }
}
