using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerInputSet input {  get; private set; }//输入
    private StateMachine stateMachine;

    public Animator anim {  get; private set; }//动画
    public Rigidbody2D rb { get; private set; }
    public Vector2 moveInput {  get; private set; }
    public Player_IdleState idleState {  get; private set; }
    public Player_MoveState moveState {  get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    private const string IDLE_ANIM_BOOL_NAME = "idle";
    private const string MOVE_ANIM_BOOL_NAME = "move";
    private const string JUMP_ANIM_BOOL_NAME = "jumpFall";
    private const string FALL_ANIM_BOOL_NAME = "jumpFall";
    private const string WALL_SLIDE_ANIM_BOOL_NAME = "wallSlide";
    private const string WALL_JUMP_ANIM_BOOL_NAME = "jumpFall";
    private const string DASH_ANIM_BOOL_NAME = "dash";
    private const string BASIC_ATTACK_ANIM_BOOL_NAME = "basicAttack";
    public bool jumpPressed;

    [Header("Attack Detail")]
    public Vector2 attackVelocity;
    public float attackVelocityDuriation = 0.1f;

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
    [HideInInspector]public float dashCoolDownTimer;

    [Header("Colliction Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallChckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected{  get; private set; }
    public bool wallDetected{  get; private set; }

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;
    private void Awake()
    {
        input = new PlayerInputSet();
        anim = GetComponentInChildren<Animator>();
        rb= GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();//定义化状态机

        idleState = new Player_IdleState(this, stateMachine, IDLE_ANIM_BOOL_NAME);//定义状态
        moveState = new Player_MoveState(this, stateMachine, MOVE_ANIM_BOOL_NAME);
        jumpState = new Player_JumpState(this, stateMachine, JUMP_ANIM_BOOL_NAME);
        fallState = new Player_FallState(this, stateMachine, FALL_ANIM_BOOL_NAME);
        wallSlideState = new Player_WallSlideState(this, stateMachine, WALL_SLIDE_ANIM_BOOL_NAME);
        wallJumpState = new Player_WallJumpState(this, stateMachine, WALL_JUMP_ANIM_BOOL_NAME);
        dashState = new Player_DashState(this, stateMachine, DASH_ANIM_BOOL_NAME);
        basicAttackState = new Player_BasicAttackState(this, stateMachine, BASIC_ATTACK_ANIM_BOOL_NAME);
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
    private void Start()
    {
        //初始化状态机的状态
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
        DashCoolDown();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    //设置速度
    public void SetVelocity(float xVelocty,float yVelocty)
    {
        rb.velocity = new Vector2(xVelocty, yVelocty);
        HandleFlip(xVelocty);
    }
    //翻转角色
    private void HandleFlip(float xVelocity)
    {
        if (xVelocity < 0 & facingRight == true)
            Flip();
        else if(xVelocity>0&facingRight == false)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }
    //地面检测
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position,Vector2.right*facingDir,wallChckDistance,whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallChckDistance * facingDir, 0, 0));
    }

    private void DashCoolDown()
    {
        dashCoolDownTimer-= Time.deltaTime;
    }
}
