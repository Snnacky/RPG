using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        GenerateAttackVelocity();
    }
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }    
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer-= Time.deltaTime;
        if(attackVelocityTimer < 0) 
            player.SetVelocity(0, rb.velocity.y);
    }
    private void GenerateAttackVelocity()
    {
        attackVelocityTimer = player.attackVelocityDuriation;
        player.SetVelocity(player.attackVelocity.x * player.facingDir, player.attackVelocity.y);
    }
}
