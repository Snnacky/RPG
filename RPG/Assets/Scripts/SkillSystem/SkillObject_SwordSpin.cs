using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int maxDistance;
    private float attacksPerSecond;
    private float attackTimer;


    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 throwPower)
    {
        anim?.SetTrigger("spin");
        base.SetupSword(swordManager, throwPower);
        maxDistance=swordManager.maxDistance;
        attacksPerSecond=swordManager.attacksPerSecond;

        Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
    }

    protected override void Update()
    {
        HandleComeback();
        HandleAttack();
        HandleStopping();
    }
    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, originalPlayer.position);
        if(distanceToPlayer>maxDistance && rb.simulated==true)
        {
            rb.simulated = false;
        }
    }

    private void HandleAttack()
    {
        attackTimer-=Time.deltaTime;
        if(attackTimer<=0)
        {
            DamageEnemiesIndius(transform, 1);
            attackTimer = 1 / attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated=false;
    }
}
