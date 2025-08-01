using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallJumpState : Player_AirState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);


    }

    public override void Update()
    {
        base.Update();
        if(rb.velocity.y<0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if(player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        
    }
}
