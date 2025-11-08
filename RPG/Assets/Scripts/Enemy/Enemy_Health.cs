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
        {
            //如果是玩家技能复制的假人攻击的敌人,则获取释放技能的玩家
            if(damageDealer.GetComponent<SkillObject_TimeEcho>()!=null)
            {
                enemy.TryEnterBattleState(damageDealer.GetComponent<SkillObject_TimeEcho>().originalPlayer);
            }else
            {
                enemy.TryEnterBattleState(damageDealer);
            }
        }
        return true;
    }
}
