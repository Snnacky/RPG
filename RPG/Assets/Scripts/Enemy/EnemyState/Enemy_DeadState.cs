using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col=enemy.GetComponent<Collider2D>();
    }

    //死亡的视觉效果
    public override void Enter()
    {
        base.Enter();
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12;
        rb.velocity = new Vector2(rb.velocity.x, 15);
        stateMachine.SwitchOffStateMachine();
        enemy.DestroyGameObjectWithDelay();
    }
}
