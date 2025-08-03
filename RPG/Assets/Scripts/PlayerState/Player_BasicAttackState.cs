using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private const int FIRST_COMBO_INDEX = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDir;
    private float lastTimeAttacked;

    private bool comboAttackQueued;
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(comboLimit!=player.attackVelocity.Length)
        {
            Debug.LogWarning("adjust limit");
            comboLimit= player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();
        anim.SetInteger("basicAttackIndex", comboIndex);

        //确定攻击方向
        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

        //赋予前摇力,需在攻击方向前,确保丝滑转身
        ApplyAttackVelocity();
    }
    
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        //在当前攻击结束前再次攻击
        if(input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();
        if(triggerCalled)//动画事件
        {
           HandleStateExit();
        }    
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    //下一个状态的判断
    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);//先false在下一帧变成true才有变化
            player.EnterAttackStateWithDelay();//推迟一帧,再执行更换状态机
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
    //在当前攻击结束前再次攻击
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }
    //趋势关闭
    private void HandleAttackVelocity()
    {
        attackVelocityTimer-= Time.deltaTime;
        if(attackVelocityTimer < 0) 
            player.SetVelocity(0, rb.velocity.y);
    }

    //使攻击有一个向前的趋势
    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuriation;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
    private void ResetComboIndexIfNeeded()
    {
        if(lastTimeAttacked + player.comboResetTime<Time.time)
            comboIndex = FIRST_COMBO_INDEX;
            
        if (comboIndex > comboLimit)
            comboIndex = FIRST_COMBO_INDEX;
    }
}
