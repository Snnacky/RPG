using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState:EntityState//定义为抽象类,确保无法直接使用
{
    protected Player player;
    protected PlayerInputSet input;
    

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();
        anim.SetFloat("yVelocity", rb.velocity.y);
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            player.dashCoolDownTimer = player.dashCoolDown;
            stateMachine.ChangeState(player.dashState);
        }
    }

    private bool CanDash()
    {
        if(player.wallDetected)
        {
            return false;
        }

        if(stateMachine.currentState == player.dashState)
        {
            return false;
        }
        if(player.dashCoolDownTimer>0)
            return false;
        return true;
    }

}
