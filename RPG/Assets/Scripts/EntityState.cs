using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState//定义为抽象类,确保无法直接使用
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;//定义peotected保证可以在子类使用
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        anim.SetFloat("yVelocity", rb.velocity.y);
        if(input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            player.dashCoolDownTimer = player.dashCoolDown;
            stateMachine.ChangeState(player.dashState);
        }
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
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
