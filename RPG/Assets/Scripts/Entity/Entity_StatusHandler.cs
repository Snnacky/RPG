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

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats= GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }

    //�׻�Ч��
    public void ApplyElectrifyEffect(float duration,float damage,float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;
        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }
        if(electrifyCo!=null)
            StopCoroutine(electrifyCo);
        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect=ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllvfx();//ֹͣ����Э��,��Ϊ��ʼ״̬
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);//����Ԥ����
        entityHealth.ReduceHp(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);
        yield return new WaitForSeconds(duration);
        //ʱ������׻�Ч��û������,����� 
        StopElectrifyEffect();
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
        entityVfx.PlayOnStatusVfx(Duration, ElementType.Ice);//����Ч��
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
