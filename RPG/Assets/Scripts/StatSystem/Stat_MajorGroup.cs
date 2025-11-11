using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat_MajorGroup 
{
    public Stat strength;//力量 每一点力量增加一点物理伤害  每俩点力量增加一点暴击伤害
    public Stat agility;//敏捷 每一点敏捷增加一点暴击率  每俩点敏捷增加一点闪避值
    public Stat intelligence;//智力 一点智力增加一点元素伤害 每俩点智力增加一点元素抗性
    public Stat vitality;//活力
}
