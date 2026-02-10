using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Thunder Claw", fileName = "Item effect data - Thunder claw")]
public class ItemEffect_ThunderClaw : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;//元素效果数据
    
    [SerializeField] private float thunderChance = .3f;
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    private void ThunderStrike(Collider2D enemy)
    {
        float random = Random.Range(0f, 1f);
        bool canAttack = random < thunderChance;
        if (canAttack == false) return;

        Entity_StatusHandler statusHandler  = enemy.GetComponent<Entity_StatusHandler>();
        statusHandler?.ApplyStatusEffect(ElementType.Lightning, effectData);
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingDamage += ThunderStrike;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingDamage -= ThunderStrike;
        player = null;
    }
}
