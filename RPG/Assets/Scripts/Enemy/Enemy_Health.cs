using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy=>GetComponent<Enemy>();
    public override bool TakeDamage(float damage,float elementalDamage,ElementType elementType, Transform damageDealer)
    {
        if (base.TakeDamage(damage,elementalDamage,elementType, damageDealer) == false)
        {
            return false;
        }

        //玩家偷袭的时候,敌人进行锁定
        if (damageDealer.CompareTag("Player"))
            enemy.TryEnterBattleState(damageDealer);
        return true;
    }
}
