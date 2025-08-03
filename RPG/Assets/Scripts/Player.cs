using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }//输入
    public Vector2 moveInput { get; private set; }


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
    public bool jumpPressed;

    [Header("Attack Detail")]
    public Vector2[] attackVelocity;
    public Vector2 fallAttackVelocity;
    public float attackVelocityDuriation = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;
    public bool jumpAttacked = false;

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

    

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();

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
    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Jump.started += ctx => jumpPressed = true;
        input.Player.Jump.canceled += ctx => jumpPressed = false;
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
}
