using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillUpgradeType
{
    None,
    //Dash Tree
    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnStart,
    Dash_ShardOnStartAndArrival,
    //Shard Tree
    Shard,
    Shard_MoveToEnemy,//��Ƭ�ƶ�ȥ����
    Shard_Multicast,//��Ƭ������������N�γ��ܡ������һ����ȫ��ʩ��
    Shard_Teleport,//����λ��
    Shard_TeleportHpRewind//����λ�ò��� ����ֵ�ٷֱ��봴����Ƭʱ��ͬ
}
