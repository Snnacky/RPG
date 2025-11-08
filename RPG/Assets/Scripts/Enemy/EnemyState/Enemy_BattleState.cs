using Unity.Mathematics;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform currentPlayer;//打算攻击的对象
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        //进入的时候已经先检查到玩家了
        base.Enter();
        UpdateBattleTimer();
        //??检测player是不是null,是就执行
       // currentPlayer ??= enemy.GetPlayerReference();//获取玩家
        currentPlayer = enemy.GetPlayerReference();//获取玩家//玩家放置timeEcho后,敌人消灭后仍然可以找到玩家
        //如果玩家离得很近,则向后退
        if (ShouldRetreat())
        {
            rb.velocity = new Vector2((enemy.retreatVelovity.x * enemy.CalculateActiveSlowMultiplier()) * -DirectionToPlayer(), enemy.retreatVelovity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();
        //检测到玩家
        if ((enemy.playerDetection() == true))
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }
        //打算攻击的时间已经到了或者玩家死亡,回到idle
        if (BattleTimeIsOver()|| currentPlayer==null)
            stateMachine.ChangeState(enemy.idleState);
        //如果玩家进入攻击范围并且检测到玩家,则攻击,
        if (WithinAttackRange() && enemy.playerDetection())
            stateMachine.ChangeState(enemy.attackState);
        else
        {
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);//改变移动速度
        } 
    }
    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    //更新最后一次检测到玩家的时间
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    //判断是否可以攻击
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    //玩家距离小于最小距离的时候
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    //敌人与玩家的距离
    private float DistanceToPlayer()
    {
        if (currentPlayer == null)
            return float.MaxValue;
        return math.abs(currentPlayer.position.x - enemy.transform.position.x);
    }
    //敌人与玩家的方向
    private int DirectionToPlayer()
    {
        if (currentPlayer == null) return 0;
        return currentPlayer.position.x > enemy.transform.position.x ? 1 : -1;
    }

    //更新攻击目标
    private void UpdateTargetIfNeeded()
    {
        if (enemy.playerDetection() == false)
        {
            return;
        }

        //检测到新目标,新目标与当前目标不一样,则更新为当前目标
        Transform newTarget = enemy.playerDetection().transform;
        if (newTarget != currentPlayer)
        {
            currentPlayer = newTarget;
        }
    }
}
