using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        
        if (rb.velocity.y < 0 && stateMachine.currentState != player.fallAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
