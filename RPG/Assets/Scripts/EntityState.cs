using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState//定义为抽象类,确保无法直接使用
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;
    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        Debug.Log("Enter" + stateName);
    }
    public virtual void Update()
    {
        Debug.Log("Update" + stateName);
    }

    public virtual void Exit()
    {
        Debug.Log("exit" + stateName);
    }

}
