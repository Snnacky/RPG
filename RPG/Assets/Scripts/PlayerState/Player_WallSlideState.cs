using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
       

        HandleWallSlide();
        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
        if (!player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            if(player.facingDir != player.moveInput.x)
                player.Flip();
        }
        
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y<0)
        {
            player.SetVelocity(player.moveInput.x,rb.velocity.y);
        }else
        {
            player.SetVelocity(player.moveInput.x, rb.velocity.y * player.wallSlideMultiplier);
        }

    }
}
