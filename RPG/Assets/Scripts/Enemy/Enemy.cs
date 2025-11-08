using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;
    [Header("Movement Detail")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;//移动速度
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }

    [Header("Battle Detail")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;//战斗检测的时间,超过这个时间没检测到人就回到idlestate
    public float minRetreatDistance = 1;
    public Vector2 retreatVelovity;

    [Header("Stunned Detail")]
    public float stunnedDuration = 1;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned = false;

    //public float activeSlowMultiplier { get; private set; } = 1;

    //public float GetMoveSpeed() => moveSpeed * CalculateActiveSlowMultiplier();
    // public float GetBattleSpeed() => battleMoveSpeed * CalculateActiveSlowMultiplier();

    #region originalSpeed
    private float originalSpeed;
    private float originalBattleSpeed;
    private float originalAnimSpeed;
    #endregion
    //改变速度
    public override void ChangeSpeed()
    {
        float activeSlowMultiplier = 1 - CalculateActiveSlowMultiplier();
        if (activeSlowMultiplier != 0)
        {
            moveSpeed = originalSpeed * activeSlowMultiplier;
            battleMoveSpeed=originalBattleSpeed * activeSlowMultiplier;
            anim.speed = originalAnimSpeed * activeSlowMultiplier;
        }
        else
        {
            moveSpeed = originalSpeed;
            battleMoveSpeed = originalBattleSpeed;
            anim.speed = originalAnimSpeed;
        }

    }

    public override float CalculateActiveSlowMultiplier()
    {
        return base.CalculateActiveSlowMultiplier();
    }


    protected override void Awake()
    {
        base.Awake();
        originalSpeed = moveSpeed;
        originalBattleSpeed = battleMoveSpeed;
        originalAnimSpeed = anim.speed;
    }
    public override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        /*
        activeSlowMultiplier = 1 - slowMultiplier;//减慢40%,相当于剩下原来的60%

        anim.speed = anim.speed * activeSlowMultiplier;

        yield return new WaitForSeconds(duration);
        StopSlowDown();
        */
        SlowEffect effect = new SlowEffect(duration, slowMultiplier);
        slowList.Add(effect);
        ChangeSpeed();
        yield return new WaitForSeconds(duration);
        slowList.Remove(effect);
        ChangeSpeed();
        // if(slowList.Count==0)
        //   StopSlowDown();
    }


    public override void StopSlowDown()
    {
        ChangeSpeed();
        base.StopSlowDown();
    }

    //是否可以被反击
    public void EnableCounterWindow(bool enable) => canBeStunned = enable;
    //处理死亡
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    //背后偷袭的时候进入battleState
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;
        this.player = player;
        if (stateMachine.currentState != battleState)
            stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
            player = playerDetection().transform;
        return player;
    }

    //玩家检测
    public RaycastHit2D playerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        return hit;
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));

    }

    private void OnEnable()
    {
        Player.OnPlayerDeathh += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeathh -= HandlePlayerDeath;
    }


}
