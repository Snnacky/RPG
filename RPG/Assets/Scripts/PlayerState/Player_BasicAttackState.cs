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

        //ȷ����������
        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

        //����ǰҡ��,���ڹ�������ǰ,ȷ��˿��ת��
        ApplyAttackVelocity();
    }
    
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        //�ڵ�ǰ��������ǰ�ٴι���
        if(input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();
        if(triggerCalled)//�����¼�
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

    //��һ��״̬���ж�
    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);//��false����һ֡���true���б仯
            player.EnterAttackStateWithDelay();//�Ƴ�һ֡,��ִ�и���״̬��
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
    //�ڵ�ǰ��������ǰ�ٴι���
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }
    //���ƹر�
    private void HandleAttackVelocity()
    {
        attackVelocityTimer-= Time.deltaTime;
        if(attackVelocityTimer < 0) 
            player.SetVelocity(0, rb.velocity.y);
    }

    //ʹ������һ����ǰ������
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
