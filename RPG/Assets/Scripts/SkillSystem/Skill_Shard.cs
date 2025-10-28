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

    private void HandleShardMoving()
    {
        HandleShardRegular();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
    }

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
            SwapPlayerAndShard();//����λ��
            SetSkillOnCoolDown();
        }
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();//��ȡ��ǰѪ��
        }
        else
        {
            SwapPlayerAndShard();//����λ��
            playerHealth.SetHealthToPercent(savedHealthPercent);//����Ѫ��
            SetSkillOnCoolDown();
        }
    }
    //����λ��
    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition=currentShard.transform.position;
        Vector3 playerPosition=player.transform.position;
        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }

    //������ը��Ƭ
    public void CreateShard()
    {
        float detonationTime = GetDetonationTime();
        GameObject shard= Instantiate(shardPerfab,transform.position,Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);
        //�����Ƭ���������ǽ���λ�úͻظ�Ѫ��,��ǰ��ը��ʱ��ǿ�ƽ�����ȴ
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard()
    {
        GameObject shard= Instantiate(shardPerfab,transform.position,Quaternion.identity);
        bool canMove = Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_Multicast);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed);
    }

    //��ըʱ��
    public float GetDetonationTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;
        return detonateTime;
    }

    private void ForceCooldown()
    {
        //��ȴ��û����
        if (OnCoolDown() == false)
        {
            SetSkillOnCoolDown();//ǿ�ƽ�����ȴ
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
