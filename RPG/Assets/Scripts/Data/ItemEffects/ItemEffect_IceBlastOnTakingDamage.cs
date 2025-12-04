using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice Blast", fileName = "Item effect data - Ice blst on taking damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;//元素效果数据
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float coolDown;
    private float lastTimeUsed = -999;
    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
        bool noCoolDown = Time.time >= lastTimeUsed + coolDown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        if(noCoolDown &&  reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed=Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamgable damagable=target.GetComponent<IDamgable>();
            if(damagable==null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);//造成伤害

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
