using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//会进行攻击动画,动画事件会触发Attacktrigger,会进行伤害判定
public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
