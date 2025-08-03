using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private int attackDir;
    private float originalGravityScale;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        attackDir = player.facingDir;
        
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = originalGravityScale;
    }
}
