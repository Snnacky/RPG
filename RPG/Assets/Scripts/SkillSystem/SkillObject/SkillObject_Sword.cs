using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;
    

    //protected Transform playerTransform;
    protected bool shouldComeback;
    protected float comebackSpeed = 20;//回来速度
    protected float maxAllowedDistance = 25;//剑与人最大距离
    protected virtual void Update()
    {
        transform.right= rb.velocity;
        HandleComeback();
    }

    public virtual void SetupSword(Skill_SwordThrow swordManager,Vector2 throwPower)
    {
        
        rb.velocity = throwPower;

        this.swordManager = swordManager;

        playerStats = swordManager.player.stats;
        damageData = swordManager.damageData;

        //playerTransform = swordManager.transform.root;
        originalPlayer = swordManager.player.transform;
    }

    public void GetSwordBackToPlayer()=>shouldComeback = true;

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, originalPlayer.position);
        if (distance > maxAllowedDistance)
            GetSwordBackToPlayer();
        if (!shouldComeback)
            return;
        transform.position = Vector2.MoveTowards(transform.position, originalPlayer.position, comebackSpeed * Time.deltaTime);
        if (distance < 0.5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesIndius(transform, 1);//伤害判定
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent=collision.transform;
    }
}
