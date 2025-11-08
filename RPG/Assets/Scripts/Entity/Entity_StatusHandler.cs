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
    //清空负面效果
    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentEffect = ElementType.None;
        entityVfx.StopAllvfx();
    }

    //应用元素攻击效果
    public void ApplyStatusEffect(ElementType element,ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChilledEffect(effectData.chillDuration, effectData.chillSlowMulltiplier);
        
        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration, effectData.totalBurnDamage);
        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }

    //雷击效果
    public void ApplyShockEffect(float duration,float damage,float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementType.Lightning);//元素抗性
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;
        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);//强力电击
            StopShockEffect();
            return;
        }
        if(ShockCo!=null)
            StopCoroutine(ShockCo);
        ShockCo = StartCoroutine(ShockEffectCo(duration));
    }
    //刷新攻击次数
    private void StopShockEffect()
    {
        currentEffect=ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllvfx();//停止所有协程,变为初始状态
    }
    //强力电击
    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);//复制预制体
        entityHealth.ReduceHp(damage);
    }

    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);//攻击颜色
        yield return new WaitForSeconds(duration);
        //时间过后雷击效果没有续上,就清空 
        StopShockEffect();
    }

    private void ApplyBurnEffect(float duration,float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration,float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int tickPerSecond = 2;//每秒灼烧次数
        int tickCount = Mathf.RoundToInt(tickPerSecond * duration);//灼烧总次数

        float damagePerTick = totalDamage / tickCount;//每次灼烧伤害
        float tickInterval = 1f / tickPerSecond;//间隔
        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        currentEffect = ElementType.None;
    }

    private void ApplyChilledEffect(float duration,float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementType.Ice);//冰抗
        float reduceDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCo(reduceDuration,slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float Duration,float slowMultiplier)
    {
        currentEffect = ElementType.Ice;
        entity.SlowDownEntity(Duration, slowMultiplier);//动画减速
        entityVfx.PlayOnStatusVfx(Duration, ElementType.Ice);//冰冻颜色效果
        yield return new WaitForSeconds(Duration);
        currentEffect = ElementType.None;
    }

    
    public bool CanBeApplied(ElementType elementType)
    {
        if(elementType==ElementType.Lightning&&currentEffect==ElementType.Lightning)
            return true;
        if (elementType == ElementType.Ice && currentEffect == ElementType.Ice)
            return true;
        //得是当前没有其他元素覆盖,才可应用
        return currentEffect == ElementType.None;
    }
}
