using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;

    public Player_SkillManager skillManager { get; private set; }
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost) => skillPoints -= cost;
    public void AddSkillPoints(int points) => skillPoints += points;


    private void Start()
    {
        UpdateAllConnections();
    }

    //解锁初始技能
    public void UnlockDefaultSkills()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
        allTreeNodes=GetComponentsInChildren<UI_TreeNode>(true);

        foreach(var node in allTreeNodes)
            node.UnlockDefaultSkill();
    }


    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()//返回所有的技能点
    {
        UI_TreeNode[] skillNodes=GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
        {
            node.Refund();
        }
    }


    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()//更新所有连接
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnection();
        }
    }
}
