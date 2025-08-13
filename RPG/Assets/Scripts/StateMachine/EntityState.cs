using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public abstract class EntityState 
{
    protected StateMachine stateMachine;
    protected string animBoolName;//定义peotected保证可以在子类使用
    protected Animator anim;
    protected Rigidbody2D rb;
    protected float stateTimer; //计数器
    protected bool triggerCalled;//anim事件触发器

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void UpdateAnimationParameters()
    {

    }
}
