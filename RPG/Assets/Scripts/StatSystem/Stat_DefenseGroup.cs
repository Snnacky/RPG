using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat_DefenseGroup 
{
    //物理防御
    public Stat armor;//护甲
    public Stat evasion;//闪避
    //法术防御
    public Stat fireResistance;
    public Stat iceResistance;
    public Stat lightningResistance;
}
