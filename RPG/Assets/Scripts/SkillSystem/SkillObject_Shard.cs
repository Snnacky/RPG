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
    //���ž��������Ŀ���ƶ�
    public void MoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }


    //���ö�ʱ��ը
    public void  SetupShard(Skill_Shard shard)
    {
        this.shard = shard;
        playerStats = shard.player.stats;
        damageScaleData = shard.damageScaleData;
        float detonationTime=shard.GetDetonationTime();

        Invoke(nameof(Explode),detonationTime);
    }

    public void SetupShard(Skill_Shard shard,float detonationTime,bool canMove,float shardSpeed)
    {
        this.shard = shard;
        playerStats = shard.player.stats;
        damageScaleData = shard.damageScaleData;

        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed);
    }

    //��ը
    public void Explode()
    {
        DamageEnemiesIndius(transform, checkRadius);//����˺�
        GameObject vfx= Instantiate(vfxPrefab, transform.position, Quaternion.identity);//ʵ����Ԥ����
        vfx.GetComponentInChildren<SpriteRenderer>().color = shard.player.vfx.GetElementColor(usedElement);
        OnExplode?.Invoke();
        Destroy(gameObject);
    }
    //����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;
        Explode();
    }

}
