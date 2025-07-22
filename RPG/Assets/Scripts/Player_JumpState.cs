using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : EntityState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocty(rb.velocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if(rb.velocity.y<0)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
