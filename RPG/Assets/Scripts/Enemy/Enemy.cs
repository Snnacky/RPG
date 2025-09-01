using System.Collections;
using System.Collections.Generic;
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
    [Range(0,2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get;private set; }

    [Header("Battle Detail")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;//战斗检测的时间,超过这个时间没检测到人就回到idlestate
    public float minRetreatDistance = 1;
    public Vector2 retreatVelovity;

    [Header("Stunned Detail")]
    public float stunnedDuration = 1;
    public Vector2 stunnedVelocity=new Vector2(7,7);
    [SerializeField]protected bool canBeStunned = false;

    //可以被反击
    public void EnableCounterWindow(bool enable) => canBeStunned = enable;
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;
        this.player = player;
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
        RaycastHit2D hit = 
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer);
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
        Gizmos.color= Color.green;
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
