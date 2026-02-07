using UnityEngine;

public class Enemy_Slime : Enemy,ICounterable
{
    private const string IDLE_ANIM_BOOL_NAME = "idle";
    private const string MOVE_ANIM_BOOL_NAME = "move";
    private const string ATTACK_ANIM_BOOL_NAME = "attack";
    private const string BATTLE_ANIM_BOOL_NAME = "battle";
    private const string Dead_ANIM_BOOL_NAME = "empty";
    private const string Stunned_ANIM_BOOL_NAME = "stunned";

    public Enemy_SlimeDeadState slime_DeathState { get; private set; }

    [Header("Slime specifics")]
    [SerializeField] private GameObject slimeToCreatePrefab;
    [SerializeField] private int amountofSlimesToCreate = 2;
    [SerializeField] private Vector2 newSlimeVelocity;

    public bool CanBeCountered => canBeStunned;

    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, IDLE_ANIM_BOOL_NAME);
        moveState = new Enemy_MoveState(this, stateMachine, MOVE_ANIM_BOOL_NAME);
        attackState = new Enemy_AttackState(this, stateMachine, ATTACK_ANIM_BOOL_NAME);
        battleState = new Enemy_BattleState(this, stateMachine, BATTLE_ANIM_BOOL_NAME);
        slime_DeathState = new Enemy_SlimeDeadState(this, stateMachine, Dead_ANIM_BOOL_NAME);
        stunnedState = new Enemy_StunnedState(this, stateMachine, Stunned_ANIM_BOOL_NAME);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        stateMachine.ChangeState(slime_DeathState);
    }

    //处理反击
    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;
        stateMachine.ChangeState(stunnedState);//进入僵直效果
    }

    public void CreateSlime()
    {
        if(slimeToCreatePrefab == null)
            return;

        for (int i = 0; i < amountofSlimesToCreate ; i++)
        {
            GameObject newSlime = Instantiate(slimeToCreatePrefab, transform.position, Quaternion.identity);

            Enemy_Slime slimeScript = newSlime.GetComponent<Enemy_Slime>();

            slimeScript.stats.AdjustStatSetup(stats.resources, stats.offense, stats.defense, .6f, 1.2f);
            slimeScript.ApplyRespawnVelocity();
            slimeScript.StartBattleStateCheck(player);
        }
        
    }

    public void ApplyRespawnVelocity()
    {
        Vector2 velocity=new Vector2(stunnedVelocity.x*Random.Range(-2,2),stunnedVelocity.y*Random.Range(1,2f));
        SetVelocity(velocity.x, velocity.y);
    }

    public void StartBattleStateCheck(Transform player)
    {
        TryEnterBattleState(player);
        InvokeRepeating(nameof(ReEnterBattleState), 0, .3f);
    }


    private void ReEnterBattleState()
    {
        if(stateMachine.currentState==battleState || stateMachine.currentState == attackState)
        {
            CancelInvoke(nameof(ReEnterBattleState));
            return;
        }

        stateMachine.ChangeState(battleState);
    }

}
