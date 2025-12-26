using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUse(Player player)
    {
        return true;
    }

    //使用物品的效果
    public virtual void ExecuteEffect()
    {

    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }
}
