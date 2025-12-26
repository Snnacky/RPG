using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Buff effect", fileName = "Item effect data - buff ")]
public class ItemEffect_Buff :ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source=Guid.NewGuid().ToString();

    

    public override bool CanBeUse(Player player)
    {
        if(player.stats.CanApplyBuffof(source))
        {
            this.player = player;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEffect()
    {
        player.stats.ApplyBuff(buffsToApply, duration, source);
        player = null;
    }
}
