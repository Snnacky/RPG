using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UpdateBattleTimer();
        //??检测player是不是null,是就执行
        player ??= enemy.GetPlayerReference();//获取玩家
        
        //如果玩家离得很近,则向后退
        if (ShouldRetreat())
        {
            rb.velocity = new Vector2(enemy.retreatVelovity.x * -DirectionToPlayer(), enemy.retreatVelovity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        if ((enemy.playerDetection()==true))
           UpdateBattleTimer();
        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);
        //如果玩家进入攻击范围并且检测到玩家,则攻击,
        if (WithinAttackRange() && enemy.playerDetection())
            stateMachine.ChangeState(enemy.attackState);
        else enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);//改变移动速度
    }
    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    //更新最后一次检测到玩家的时间
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    //判断是否可以攻击
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    //玩家距离小于最小距离的时候,emey
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    //敌人与玩家的距离
    private float  DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;
        return math.abs(player.position.x - enemy.transform.position.x);
    }
    //敌人与玩家的方向
    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
