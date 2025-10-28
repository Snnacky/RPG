using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    private void Awake()
    {
        dash=GetComponentInChildren<Skill_Dash>();
        shard=GetComponentInChildren<Skill_Shard>();
    }

    //通过类型寻找技能
    public Skill_Base GetSkillByType(SkillType type)
    {
        switch(type)
        {
            case SkillType.Dash:return dash;
            case SkillType.TimeShard:return shard;
            default:
                Debug.Log($"Skill type{type} is not implemented yet");
                return null;
        }
    }
}
