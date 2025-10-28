using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{

    private float originalGravityScale;
    private int dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        skillManager.dash.OnStartEffect();//��ʼ���ʱ��Ч��
        player.vfx.DoImageEchoEffect(player.dashDuration);//dash��ӰЧ��

        dashDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;
        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(player.dashSpeed * dashDir, 0);
        CancleDashIfNeeded();
        if (stateTimer<0)
        {
            if(player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
        
    }
    public override void Exit()
    {
        base.Exit();
        skillManager.dash.OnendEffect();//����˳�ʱ��Ч��
        player.SetVelocity(0, 0);
        rb.gravityScale= originalGravityScale;
    }

    private void CancleDashIfNeeded()
    {
        if(player.wallDetected)
        {
            if(player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
    }
}
