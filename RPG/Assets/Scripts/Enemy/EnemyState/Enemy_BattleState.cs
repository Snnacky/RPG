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
        //??���player�ǲ���null,�Ǿ�ִ��
        player ??= enemy.GetPlayerReference();//��ȡ���
        
        //��������úܽ�,�������
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
        //�����ҽ��빥����Χ���Ҽ�⵽���,�򹥻�,
        if (WithinAttackRange() && enemy.playerDetection())
            stateMachine.ChangeState(enemy.attackState);
        else enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);//�ı��ƶ��ٶ�
    }
    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    //�������һ�μ�⵽��ҵ�ʱ��
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    //�ж��Ƿ���Թ���
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    //��Ҿ���С����С�����ʱ��,emey
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    //��������ҵľ���
    private float  DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;
        return math.abs(player.position.x - enemy.transform.position.x);
    }
    //��������ҵķ���
    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
