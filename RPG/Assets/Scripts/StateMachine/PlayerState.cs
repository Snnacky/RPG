using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState:EntityState//定义为抽象类,确保无法直接使用
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
        skillManager = player.skillManager;
    }

    public override void Update()
    {
        base.Update();
       
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCoolDown();
            player.dashCoolDownTimer = player.dashCoolDown;
            stateMachine.ChangeState(player.dashState);
        }

        if(input.Player.UltimateSpell.WasPressedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if (skillManager.domainExpansion.InstantDomain())
                skillManager.domainExpansion.CreateDomain();
            else
            {
                stateMachine.ChangeState(player.domainExpansionState);
            }
            skillManager.domainExpansion.SetSkillOnCoolDown();
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    private bool CanDash()
    {
        if(skillManager.dash.CanUseSkill()==false)
            return false;

        if(player.wallDetected)
        {
            return false;
        }

        if(stateMachine.currentState == player.dashState)
        {
            return false;
        }
        //if(player.dashCoolDownTimer>0)
         //   return false;
        return true;
    }

}
