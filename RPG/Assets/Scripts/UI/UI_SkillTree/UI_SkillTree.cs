using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour,ISaveable
{
    [SerializeField] private int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    [SerializeField] private GameObject skillBarParent;
    private UI_TreeNode[] allTreeNodes;

    public Player_SkillManager skillManager { get; private set; }

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }
    public void AddSkillPoints(int points)
    {
        skillPoints += points;
        UpdateSkillPointsUI() ;
    }


    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = skillPoints.ToString();
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
            node.isLimited = false;
        }
        //技能栏应该更新
        //skillmanager应该更新

        skillBarParent.GetComponent<UI_SkillBarParent>().RecoverySkillSlot();//恢复技能栏
        skillManager.RecoveryAllSkills();//恢复出厂设置
    }


    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()//更新所有连接
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnection();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints;

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;

            if(data.skillTreeUI.TryGetValue(skillName,out bool unlocked)&& unlocked)
                node.UnlockWithLoadData();
        }

        foreach(var skill in skillManager.allskills)
        {
            if(data.skillUpgrades.TryGetValue(skill.GetSkillType(),out SkillUpgradeType upgradeType))
            {
                var upgradeNode=allTreeNodes.FirstOrDefault(node=>node.skillData.upgradeData.upgradeType == upgradeType);
                if (upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            Debug.Log(skillName + ": " + node.isUnlocked);
            data.skillTreeUI[skillName] = node.isUnlocked;
        }

        //player身上
        foreach(var skill in skillManager.allskills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}
