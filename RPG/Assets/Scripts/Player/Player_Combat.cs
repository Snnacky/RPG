using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat :Entity_Combat
{
    [Header("Counter attack Details")]
    [SerializeField] private float counterRecovery=0.1f;
    public bool CounterAttackEnd()
    {
        bool hasCounterSomeBody = false;
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterabe = target.GetComponent<ICounterable>();//skeleton
            if (counterabe == null)
                continue;
            if(counterabe.CanBeCountered)
            {
                hasCounterSomeBody = true;
                counterabe?.HandleCounter();//½øÈë½©Ö±
            }
        }
        return hasCounterSomeBody; 
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
