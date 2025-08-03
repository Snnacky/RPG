using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallAttackState : PlayerState
{
    private bool touchedGround;
    private int attackDir;
    public Player_FallAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        attackDir = player.facingDir;
        touchedGround = false;
        player.SetVelocity(player.fallAttackVelocity.x * attackDir, player.fallAttackVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(player.groundDetected && !touchedGround)
        {
            touchedGround = true;
            anim.SetTrigger("fallAttackTrigger");
            player.SetVelocity(0, rb.velocity.y);
        }
        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
    
