using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState currentState {  get; private set; }//����ֻ��,���ڿ�д
    public bool canEnterNewState;

    //��ʼ��״̬
    public void Initialize(EntityState startState)
    {
        canEnterNewState = true;
        currentState = startState;
        currentState.Enter();
    }

    //�ı�״̬
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
