using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Shard :SkillObject_Base
{
    public event Action OnExplode;
    private Skill_Shard shard;

    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    //朝着距离最近的目标移动
    public void MoveTowardsClosestTarget(float speed ,Transform newTarget = null)
    {
        target = newTarget == null ? FindClosestTarget() : newTarget;
        this.speed = speed;
    }


    //初始化
    public void  SetupShard(Skill_Shard shard)
    {
        this.shard = shard;
        playerStats = shard.player.stats;
        damageData = shard.damageData;
        originalPlayer = shard.player.transform;
        float detonationTime=shard.GetDetonationTime();

        Invoke(nameof(Explode),detonationTime); //设置定时爆炸
    }

    public void SetupShard(Skill_Shard shard,float detonationTime,bool canMove,float shardSpeed,Transform target=null)
    {
        this.shard = shard;
        playerStats = shard.player.stats;
        damageData = shard.damageData;
        originalPlayer = shard.player.transform;
        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed, target);
    }

    //爆炸
    public void Explode()
    {
        DamageEnemiesIndius(transform, attackCheckRadius);//造成伤害
        GameObject vfx= Instantiate(vfxPrefab, transform.position, Quaternion.identity);//实例化预制体
        vfx.GetComponentInChildren<SpriteRenderer>().color = shard.player.vfx.GetElementColor(usedElement);
        OnExplode?.Invoke();
        Destroy(gameObject);
    }
    //触发器触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;
        Explode();
    }

}
