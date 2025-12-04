using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;
    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;//解锁所需node
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;//已经解开了
    public bool isLocked;//被锁住了(技能冲突)

    [Header("Skill details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#726B6B";//锁住的颜色
    private Color lastColor;

    //当inspector里面的值被修改时会调用
    private void OnValidate()
    {
        if (skillData == null)
        {
            return;
        }
        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
        UpdateIconColor(GetColorByHex(lockedColorHex));//初始化颜色
    }

    private void Start()
    {
        //默认解锁
        if (skillData.unlockedByDefualt)
            Unlock();
        
    }

    private void OnEnable()
    {
        //没有被解开
        if(isUnlocked==false)
            UpdateIconColor(GetColorByHex(lockedColorHex));

    }

    public void Refund()//返回技能点
    {
        if (isUnlocked == false || skillData.unlockedByDefualt)
            return;
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);//增加技能点
        connectHandler.UnlockConnectionImage(false);//解锁连接线颜色
    }

    //解锁
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);//花费支出
        connectHandler.UnlockConnectionImage(true);//改变连接线颜色

        //获取skill_base,再设置升级类型
        skillTree.skillManager.GetSkillBaseByType(skillData.skillType).
            SetSkillUpgrade(skillData);
    }

    //检查是否可以解锁
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;
        if (skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }
        return true;
    }

    //锁定冲突技能
    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();//锁定冲突技能以下的所有技能
        }
    }
    //锁定冲突技能以下的所有技能
    public void LockChildNodes()
    {
        isLocked = true;
        foreach (var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }

    //改变技能图片颜色
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    //鼠标点击区域
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();

    }

    //鼠标点进区域
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;
        ToggleNodeHighlight(true);//高亮


    }
    //鼠标离开
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);//关闭高亮
    }
    //高光
    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;

        UpdateIconColor(colorToApply);
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));
        if (isUnlocked)
            UpdateIconColor(Color.white);
    }
}
