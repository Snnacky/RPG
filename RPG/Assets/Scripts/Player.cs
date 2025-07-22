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

    private const string IDLE_ANIM_BOOL_NAME = "idle";
    private const string MOVE_ANIM_BOOL_NAME = "move";
    private const string JUMP_ANIM_BOOL_NAME = "jumpFall";
    private const string FALL_ANIM_BOOL_NAME = "jumpFall";

    public bool jumpPressed;

    [Header("Movement Detail")]
    public float moveSpeed;
    public float jumpForce = 5;

    private bool facingRight = true;
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
        stateMachine.UpdateActiveState();
        
    }

    public void SetVelocty(float xVelocty,float yVelocty)
    {
        rb.velocity = new Vector2(xVelocty, yVelocty);
        HandleFlip(xVelocty);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity < 0 & facingRight == true)
            Flip();
        else if(xVelocity>0&facingRight == false)
            Flip();
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}
