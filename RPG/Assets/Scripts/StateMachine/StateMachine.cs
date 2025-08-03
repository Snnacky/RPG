using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState currentState {  get; private set; }//����ֻ��,���ڿ�д


    //��ʼ��״̬
    public void Initialize(EntityState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    //�ı�״̬
    public void ChangeState(EntityState newState)
    {
        currentState.Exit();
        currentState = newState;    
        currentState.Enter();
    }


    public void UpdateActiveState()
    {
        currentState.Update();
    }
}
