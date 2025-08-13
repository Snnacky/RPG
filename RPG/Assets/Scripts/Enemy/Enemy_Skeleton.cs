using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    private const string IDLE_ANIM_BOOL_NAME = "idle";
    private const string MOVE_ANIM_BOOL_NAME = "move";
    private const string ATTACK_ANIM_BOOL_NAME = "attack";
    private const string BATTLE_ANIM_BOOL_NAME = "battle";
    private const string Dead_ANIM_BOOL_NAME = "empty";
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, IDLE_ANIM_BOOL_NAME);
        moveState = new Enemy_MoveState(this, stateMachine, MOVE_ANIM_BOOL_NAME);
        attackState = new Enemy_AttackState(this, stateMachine, ATTACK_ANIM_BOOL_NAME);
        battleState = new Enemy_BattleState(this, stateMachine, BATTLE_ANIM_BOOL_NAME);
        deadState = new Enemy_DeadState(this, stateMachine, Dead_ANIM_BOOL_NAME);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
}
