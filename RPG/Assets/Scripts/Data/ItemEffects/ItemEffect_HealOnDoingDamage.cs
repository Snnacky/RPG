using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal on doing damage", fileName = "Item effect data - heal on doing physical damage ")]
public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealOnAttack = .2f;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage*percentHealOnAttack);
    }
}
