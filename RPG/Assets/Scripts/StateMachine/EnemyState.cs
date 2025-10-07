using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        rb = enemy.rb;
        anim=enemy.anim;
        stats = enemy.stats;
    }


    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        anim.SetFloat("battleSpeedMultiplier", battleAnimSpeedMultiplier);//可以更改move动画速度
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);//可以更改move动画速度
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
