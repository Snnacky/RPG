using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat=GetComponentInParent<Entity_Combat>();
    }

    //动画事件
    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
    
    //动画事件
    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
