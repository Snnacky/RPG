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
        //ʹ�ÿ����ڿ����ƶ�
        if(player.moveInput.x!=0 && stateMachine.currentState!=player.wallJumpState)
        {
            player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.velocity.y);
        }
        //�ع�������(���乥��)
        if (input.Player.HeavyAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.fallAttackState);
        //���չ���
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
