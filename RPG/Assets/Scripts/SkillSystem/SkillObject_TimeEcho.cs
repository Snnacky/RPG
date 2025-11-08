using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject onDeathVfx;

    private bool shouldMoveToPlayer;
    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;

    private Entity_Health playerHealth;
    private SkillObject_Health echoHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;

    public int maxAttacks {  get; private set; }//最大攻击次数

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();
        if(target!=null && target.position.x<transform.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    //初始化
    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        originalPlayer = echoManager.player.transform;
        playerHealth= echoManager.player.health;
        skillManager = echoManager.skillManager;
        statusHandler = echoManager.player.statusHandler;
        echoHealth = GetComponent<SkillObject_Health>();

        FlipToTarget();

        maxAttacks=echoManager.GetMaxAttacks();//获取攻击次数
        anim.SetBool("canAttack", maxAttacks > 0);
        Invoke(nameof(HandleDeath),echoManager.GetEchoDuration());

        wispTrail=GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.velocity.y);
            StopHorizontalMovement();
        }    
            
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, originalPlayer.position, wispMoveSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, originalPlayer.position)<0.5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
        //治疗的血量
        float healAmount=echoHealth.lastDamageTaken*echoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);
        //减少技能冷却时间
        float amountInSeconds=echoManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);
        //消除负面影响
        if(echoManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects();
    }

    public void PerformAttack()
    {
        DamageEnemiesIndius(targetCheck, 1);
        if (targetGotHit == false)
            return;
        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;
        if(canDuplicate)
        {
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
        }
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx,transform.position, Quaternion.identity);
        if(echoManager.ShouldBeWisp())
        {
            TurnIntoWisp();
        }
        else
        {
            Destroy(gameObject);

        }
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    //固定
    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);
        if(hit.collider!=null)
            rb.velocity=new Vector2(0,rb.velocity.y);
    }
}
