using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeathh;

    private UI ui;
    public PlayerInputSet input { get; private set; }//输入
    public Vector2 moveInput { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    public Vector2 mousePosition { get; private set; }
    #region Stat Variables
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_FallAttackState fallAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }

    private const string IDLE_ANIM_BOOL_NAME = "idle";
    private const string MOVE_ANIM_BOOL_NAME = "move";
    private const string JUMP_ANIM_BOOL_NAME = "jumpFall";
    private const string FALL_ANIM_BOOL_NAME = "jumpFall";
    private const string WALL_SLIDE_ANIM_BOOL_NAME = "wallSlide";
    private const string WALL_JUMP_ANIM_BOOL_NAME = "jumpFall";
    private const string DASH_ANIM_BOOL_NAME = "dash";
    private const string BASIC_ATTACK_ANIM_BOOL_NAME = "basicAttack";
    private const string FALL_ATTACK_ANIM_BOOL_NAME = "fallAttack";
    private const string JUMP_ATTACK_ANIM_BOOL_NAME = "jumpAttack";
    private const string DEAD_ATTACK_ANIM_BOOL_NAME = "dead";
    private const string COUNTER_ATTACK_ANIM_BOOL_NAME = "counterAttack";
    private const string SWORD_THROW_ANIM_BOOL_NAME = "swordThrow";
    private const string DOMAIN_EXPANSION_ANIM_BOOL_NAME = "jumpFall";

    #endregion
    public bool jumpPressed;

    [Header("Attack Detail")]
    public Vector2[] attackVelocity;
    public Vector2 fallAttackVelocity;
    public float attackVelocityDuriation = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;
    public bool jumpAttacked = false;

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;

    [Header("Movement Detail")]
    public float moveSpeed;//移动速度
    public float jumpForce = 5;//跳跃力
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMoveMultiplier;//空气乘数
    [Range(0, 1)]
    public float wallSlideMultiplier;//墙体下滑乘数
    [Space]
    public float dashDuration = 0.2f;
    public float dashSpeed = 20;
    public float dashCoolDown;
    [HideInInspector] public float dashCoolDownTimer;

    #region originalSpeed
    float originalMoveSpeed;
    float originalJumpForce;
    float originalAnimSpeed;
    Vector2 originalWallJump;
    Vector2 originalJumpAttack;
    Vector2[] originalAttackVelocity;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        ui=FindAnyObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();

        originalMoveSpeed = moveSpeed;
        originalJumpForce = jumpForce;
        originalAnimSpeed = anim.speed;
        originalWallJump = wallJumpForce;
        originalJumpAttack = fallAttackVelocity;
        originalAttackVelocity = attackVelocity;


        idleState = new Player_IdleState(this, stateMachine, IDLE_ANIM_BOOL_NAME);//定义状态
        moveState = new Player_MoveState(this, stateMachine, MOVE_ANIM_BOOL_NAME);
        jumpState = new Player_JumpState(this, stateMachine, JUMP_ANIM_BOOL_NAME);
        fallState = new Player_FallState(this, stateMachine, FALL_ANIM_BOOL_NAME);
        wallSlideState = new Player_WallSlideState(this, stateMachine, WALL_SLIDE_ANIM_BOOL_NAME);
        wallJumpState = new Player_WallJumpState(this, stateMachine, WALL_JUMP_ANIM_BOOL_NAME);
        dashState = new Player_DashState(this, stateMachine, DASH_ANIM_BOOL_NAME);
        basicAttackState = new Player_BasicAttackState(this, stateMachine, BASIC_ATTACK_ANIM_BOOL_NAME);
        fallAttackState = new Player_FallAttackState(this, stateMachine, FALL_ATTACK_ANIM_BOOL_NAME);
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, JUMP_ATTACK_ANIM_BOOL_NAME);
        deadState = new Player_DeadState(this, stateMachine, DEAD_ATTACK_ANIM_BOOL_NAME);
        counterAttackState = new Player_CounterAttackState(this, stateMachine, COUNTER_ATTACK_ANIM_BOOL_NAME);
        swordThrowState = new Player_SwordThrowState(this, stateMachine, SWORD_THROW_ANIM_BOOL_NAME);
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, DOMAIN_EXPANSION_ANIM_BOOL_NAME);
    }

    public override void ChangeSpeed()
    {
        //计算减速值
        float activeSlowMultiplier = 1-CalculateActiveSlowMultiplier();

        //减速
        if (activeSlowMultiplier != 0)
        {
            moveSpeed = originalMoveSpeed* activeSlowMultiplier;
            jumpForce = originalJumpForce * activeSlowMultiplier;
            wallJumpForce = originalWallJump * activeSlowMultiplier;
            fallAttackVelocity = originalJumpAttack * activeSlowMultiplier;
            for (int i = 0; i < attackVelocity.Length; i++)
            {
                attackVelocity[i] = originalAttackVelocity[i] * activeSlowMultiplier;
            }
            anim.speed = originalAnimSpeed * activeSlowMultiplier;
        }
        //恢复
        else
        {
            moveSpeed = originalMoveSpeed;
            jumpForce = originalJumpForce;
            wallJumpForce = originalWallJump;
            fallAttackVelocity = originalJumpAttack;
            for (int i = 0; i < attackVelocity.Length; i++)
            {
                attackVelocity[i] = originalAttackVelocity[i];
            }
            anim.speed = originalAnimSpeed;
        }
    }
    public override float CalculateActiveSlowMultiplier()
    {
        return base.CalculateActiveSlowMultiplier();
    }

    public override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        /*
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = fallAttackVelocity;
        Vector2[] originalAttackVelocity= attackVelocity;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        fallAttackVelocity = fallAttackVelocity * speedMultiplier;

        for(int i=0; i<attackVelocity.Length; i++)
        {
            attackVelocity[i]=attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        fallAttackVelocity = originalJumpAttack;

        for(int i=0;i<attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
        */

        SlowEffect effect = new SlowEffect(duration, slowMultiplier);
        slowList.Add(effect);
        ChangeSpeed();
        yield return new WaitForSeconds(duration);
        slowList.Remove(effect);
        ChangeSpeed();
    }

    public void EnterAttackStateWithDelay()
    {
        //如果前面执行了协程还没结束,则结束,再执行下一个协程
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);
        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeathh?.Invoke();
        stateMachine.ChangeState(deadState);
    }
    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Jump.started += ctx => jumpPressed = true;
        input.Player.Jump.canceled += ctx => jumpPressed = false;

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();//开关技能ui
        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();//爆炸碎片攻击
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();//复制player进行攻击

        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override void Start()
    {
        base.Start();
        //初始化状态机的状态
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        DashCoolDown();
    }
    private void DashCoolDown()
    {
        dashCoolDownTimer -= Time.deltaTime;
    }

    public void TeleportPlayer(Vector3 position)=>transform.position = position;//变换位置
}
