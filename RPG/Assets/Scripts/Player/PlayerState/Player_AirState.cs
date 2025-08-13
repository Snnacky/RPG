using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AirState : PlayerState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        //使得可以在空中移动
        if(player.moveInput.x!=0 && stateMachine.currentState!=player.wallJumpState)
        {
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.velocity.y);
        }
        //重攻击触发(下落攻击)
        if (input.Player.HeavyAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.fallAttackState);
        //浮空攻击
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
