using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (enemy.playerDetection() == true)
            stateMachine.ChangeState(enemy.battleState);
    }
}
