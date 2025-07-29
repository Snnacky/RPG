using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundedState : EntityState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(input.Player.Jump.WasPerformedThisFrame())
        {
           stateMachine.ChangeState(player.jumpState);
        }
        if (rb.velocity.y < 0 && !player.groundDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if(input.Player.Attack.WasPerformedThisFrame()) 
            stateMachine.ChangeState(player.basicAttackState);
        if(player.jumpAttacked)
            player.jumpAttacked = false;
    }
}
