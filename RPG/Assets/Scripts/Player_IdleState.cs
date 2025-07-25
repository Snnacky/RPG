using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y<0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.moveInput.x!=0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    { 
        base.Exit(); 
        
    }
}
