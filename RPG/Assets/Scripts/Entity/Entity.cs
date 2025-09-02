using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;



    protected StateMachine stateMachine;
    public Animator anim { get; private set; }//����
    public Rigidbody2D rb { get; private set; }

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    //����
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
        stateMachine = new StateMachine();//���廯״̬��
        
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
    //Э��:����Ч��
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

    //����
    public virtual void EntityDeath()
    {

    }

    //�����ٶ�
    public void SetVelocity(float xVelocty, float yVelocty)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(xVelocty, yVelocty);
        HandleFlip(xVelocty);
    }
    //��ת��ɫ
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
    //������
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
