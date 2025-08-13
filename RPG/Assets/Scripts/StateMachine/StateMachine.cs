using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState currentState {  get; private set; }//对外只读,对内可写
    public bool canEnterNewState;

    //初始化状态
    public void Initialize(EntityState startState)
    {
        canEnterNewState = true;
        currentState = startState;
        currentState.Enter();
    }

    //改变状态
    public void ChangeState(EntityState newState)
    {
        if (canEnterNewState == false)
            return;
        currentState.Exit();
        currentState = newState;    
        currentState.Enter();
    }


    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => canEnterNewState = false;

}
