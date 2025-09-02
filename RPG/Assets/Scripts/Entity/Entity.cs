using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;



    protected StateMachine stateMachine;
    public Animator anim { get; private set; }//动画
    public Rigidbody2D rb { get; private set; }

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    //击退
    private Coroutine knockbackCo;
    private bool isKnocked;

    [Header("Colliction Detection")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallChckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondarWallCheck;
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();//定义化状态机
        
    }
  
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
        
    }

    protected virtual void Start()
    {

    }
    public void ReciveKnockback(Vector2 knockback,float duration)
    {
        if(knockbackCo!=null)
            StopCoroutine(knockbackCo);
        knockbackCo = StartCoroutine(KnockbackCo(knockback,duration));
    }
    //协程:击退效果
    private IEnumerator KnockbackCo(Vector2 knockback,float duration)
    {
        isKnocked = true;
        rb.velocity = knockback;
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero;
        isKnocked = false;
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    //死亡
    public virtual void EntityDeath()
    {

    }

    //设置速度
    public void SetVelocity(float xVelocty, float yVelocty)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(xVelocty, yVelocty);
        HandleFlip(xVelocty);
    }
    //翻转角色
    public void HandleFlip(float xVelocity)
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

        OnFlipped?.Invoke();
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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallChckDistance * facingDir, 0, 0));
        if(secondarWallCheck != null)
            Gizmos.DrawLine(secondarWallCheck.position, secondarWallCheck.position + new Vector3(wallChckDistance * facingDir, 0, 0));
    }

    
}
