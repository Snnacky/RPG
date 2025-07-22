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
    }

    public override void Update()
    {
        base.Update();
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
