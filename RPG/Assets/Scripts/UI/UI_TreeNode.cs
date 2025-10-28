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
    public UI_TreeNode[] neededNodes;//��������node
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;//�Ѿ��⿪��
    public bool isLocked;//����ס��

    [Header("Skill details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#726B6B";//��ס����ɫ
    private Color lastColor;

    //��inspector�����ֵ���޸�ʱ�����
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
        UpdateIconColor(GetColorByHex(lockedColorHex));//��ʼ����ɫ
    }

    private void Start()
    {
        //Ĭ�Ͻ���
        if (skillData.unlockedByDefualt)
            Unlock();
        
    }

    public void Refund()//���ؼ��ܵ�
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);//���Ӽ��ܵ�
        connectHandler.UnlockConnectionImage(false);//������������ɫ
    }

    //����
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);//����֧��
        connectHandler.UnlockConnectionImage(true);//�ı���������ɫ

        //��ȡskill_base,��������������
        skillTree.skillManager.GetSkillByType(skillData.skillType).
            SetSkillUpgrade(skillData.upgradeData);
    }

    //����Ƿ���Խ���
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

    //������ͻ����
    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();//������ͻ�������µ����м���
        }
    }
    //������ͻ�������µ����м���
    public void LockChildNodes()
    {
        isLocked = true;
        foreach (var node in connectHandler.GetChildNodes())
            node.LockChildNodes();
    }

    //�ı似��ͼƬ��ɫ
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    //���������
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();

    }

    //���������
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked || isLocked)
            return;
        ToggleNodeHighlight(true);//����


    }
    //����뿪
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);//�رո���
    }
    //�߹�
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
