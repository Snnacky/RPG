using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool needToBeReCalucated = true;
    private float finalValue;
    public float GetValue()
    {
        if(needToBeReCalucated)
        {
            finalValue=GetFinalValue();
            needToBeReCalucated=false;
        }
        return finalValue;
    }

    public void AddModifier(float value,string source)
    {
        StatModifier modToAdd = new StatModifier(value,source);
        modifiers.Add(modToAdd);
        needToBeReCalucated = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);//删除所有source一样的
        needToBeReCalucated = true;
    }

    //获取增加buff后的值
    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value)=>baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
