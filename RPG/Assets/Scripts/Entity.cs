using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    
    protected StateMachine stateMachine;
    public Animator anim { get; private set; }//动画
    public Rigidbody2D rb { get; private set; }

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Colliction Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallChckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondarWallCheck;
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();//定义化状态机
        
    }
  
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
        
    }
    
    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    //设置速度
    public void SetVelocity(float xVelocty, float yVelocty)
    {
        rb.velocity = new Vector2(xVelocty, yVelocty);
        HandleFlip(xVelocty);
    }
    //翻转角色
    private void HandleFlip(float xVelocity)
    {
        if (xVelocity < 0 & facingRight == true)
            Flip();
        else if (xVelocity > 0 & facingRight == false)
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        if (secondarWallCheck != null)
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallChckDistance, whatIsGround)
                && Physics2D.Raycast(secondarWallCheck.position, Vector2.right * facingDir, wallChckDistance, whatIsGround);
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallChckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallChckDistance * facingDir, 0, 0));
        if(secondarWallCheck != null)
            Gizmos.DrawLine(secondarWallCheck.position, secondarWallCheck.position + new Vector3(wallChckDistance * facingDir, 0, 0));
    }

    
}
