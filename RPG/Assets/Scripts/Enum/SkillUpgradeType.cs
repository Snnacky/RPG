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
    Shard_MoveToEnemy,//碎片移动去敌人
    Shard_Multicast,//碎片能力最多可以有N次充能。你可以一次性全部施放
    Shard_Teleport,//交换位置
    Shard_TeleportHpRewind,//交换位置并且 生命值百分比与创建碎片时相同
    //Sword Throw
    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,
    //TimeEcho
    TimeEcho,
    TimeEcho_singleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_ChanceToDuplicate,
    TimeEcho_HealWisp,
    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,
    //Domain Expansion
    Domain_SlowingDown,
    Domain_EchoSpam,
    Domain_ShardSpam
}
