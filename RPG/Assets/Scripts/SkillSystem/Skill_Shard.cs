using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPerfab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isReCharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth=GetComponentInParent<Entity_Health>();     
    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();
        if (CanUseSkill() == false)
            return;
        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        if(Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();
        if(Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleShardMulticast();
        if(Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();
        if(Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }
    //晶体移动
    private void HandleShardMoving()
    {
        HandleShardRegular();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
    }

    //多个晶体
    private void HandleShardMulticast()
    {
        if (currentCharges <= 0) return;
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;
        if (isReCharging == false)
            StartCoroutine(ShardReChargeCo());
    }

    private IEnumerator ShardReChargeCo()
    {
        isReCharging = true;
        while(currentCharges<maxCharges)
        {
            currentCharges++;
            yield return new WaitForSeconds(cooldown);
        }
        isReCharging = false;
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCoolDown();
    }

    private void HandleShardTeleport()
    {
        if(currentShard==null)
            CreateShard();
        else
        {
            SwapPlayerAndShard();//交换位置
            SetSkillOnCoolDown();
        }
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();//获取当前血量
        }
        else
        {
            SwapPlayerAndShard();//交换位置
            playerHealth.SetHealthToPercent(savedHealthPercent);//更新血量
            SetSkillOnCoolDown();
        }
    }
    //交换位置
    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition=currentShard.transform.position;
        Vector3 playerPosition=player.transform.position;
        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }

    //创建爆炸碎片
    public void CreateShard()
    {
        float detonationTime = GetDetonationTime();
        GameObject shard= Instantiate(shardPerfab,transform.position,Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);
        //如果碎片进化方向是交换位置和回复血量,提前爆炸的时候强制进入冷却
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }
    //创建冲刺碎片和黑洞碎片
    public void CreateRawShard(Transform target = null, bool shardsCanMove = false)
    {
        bool canMove = shardsCanMove != false ? shardsCanMove :
            Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_Multicast);

        GameObject shard= Instantiate(shardPerfab,transform.position,Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed, target);
    }

    //爆炸时间
    public float GetDetonationTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;
        return detonateTime;
    }

    private void ForceCooldown()
    {
        //冷却还没结束
        if (OnCoolDown() == false)
        {
            SetSkillOnCoolDown();//强制进入冷却
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
