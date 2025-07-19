using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMachine stateMachine;

    public Player_IdleState idleState {  get; private set; }
    public Player_MoveState moveState {  get; private set; }
    private void Awake()
    {
        stateMachine = new StateMachine();//定义化状态机

        idleState = new Player_IdleState(this, stateMachine, "IdleState");//定义状态
        moveState = new Player_MoveState(this, stateMachine, "MoveState");
    }

    private void Start()
    {
        //初始化状态机的状态
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    
}
