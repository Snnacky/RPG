using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] private float bounceSpeed;
    private int bounceCount;

    private Collider2D[] enemyTargets;
    private Transform nextPlayer;
    private Transform lastPlayer;

    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 throwPower)
    {
        base.SetupSword(swordManager, throwPower);
       
        bounceSpeed=swordManager.bounceSpeed;
        bounceCount=swordManager.bounceCount;

        anim.SetTrigger("spin");
    }

    protected override void Update()
    {
        HandleComeback();
        HandleBounce();

    }

    private void HandleBounce()
    {
        if (nextPlayer == null)
            return;
        transform.position=Vector2.MoveTowards(transform.position,nextPlayer.position,bounceSpeed*Time.deltaTime);

        if(Vector2.Distance(transform.position,nextPlayer.position)<.75f)
        {
            DamageEnemiesIndius(transform, 1);//伤害
            lastPlayer = nextPlayer;//记录该次攻击对象
            BounceToNextTarget();//计算下一个攻击对象,攻击次数减1
            if(bounceCount==0 || nextPlayer==null)
            {
                nextPlayer = null;
                lastPlayer = null;
                GetSwordBackToPlayer();
            }
        }
    }
    //计算下一个攻击对象,攻击次数减1
    private void BounceToNextTarget()
    {
        nextPlayer=GetNextTarget();
        bounceCount--;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = GetEnemiersAround(transform, 10);
            rb.simulated = false;
        }

        DamageEnemiesIndius(transform, 1);
        if (enemyTargets.Length <=1  || bounceCount == 0)//范围内只有一个敌人
            GetSwordBackToPlayer();
        else
            nextPlayer = GetNextTarget();
    }

    //获取下一个攻击对象
    private Transform GetNextTarget()
    {
        List<Transform> validTarget=GetValidTargets();
        //如果没有可攻击对象,返回空
        if (validTarget==null)
            return null;

        int randomIndex=Random.Range(0,validTarget.Count);

        Transform nextTarget = validTarget[randomIndex];

        return nextTarget;
    }

    //获取可以攻击的敌人
    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets=new List<Transform>();
        List<Transform> aliveTargets=GetAliveTargets();//获取范围内所以的存活对象

        foreach (var enemy in aliveTargets)
        {
            if(enemy==null)
                aliveTargets.Remove(enemy);
            else
            {
                //筛选出没有上次攻击过的敌人
                if(lastPlayer != enemy)
                    validTargets.Add(enemy.transform);
            }
        }
        if(validTargets.Count>0)
            return validTargets;
        else
        {
            return null;
        }
    }

    //获取攻击范围内的所有存活敌人
    private List<Transform> GetAliveTargets()
    {
        List<Transform> aliveTargets = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if(enemy!=null && enemy.GetComponent<Entity_Health>().isDead == false)
            {
                aliveTargets.Add(enemy.transform);
            }
        }
        return aliveTargets;
    }


}
