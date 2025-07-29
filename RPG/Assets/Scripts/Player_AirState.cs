using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AirState : EntityState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(player.moveInput.x!=0)
        {
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.velocity.y);
        }
        if (input.Player.HeavyAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.fallAttackState);
        if (input.Player.Attack.WasPressedThisFrame())
        { 
            if(player.jumpAttacked==false)
            {
                player.jumpAttacked = true;
                stateMachine.ChangeState(player.jumpAttackState);
            }
           
        }
    }
}
