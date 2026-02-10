using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skillSlots;//技能栏

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Quick Item Slots")]
    [SerializeField] private float yoffsetQuickItemParent = 200;
    [SerializeField] private Transform quickItemOptionsParent;
    private UI_QuickItemSlotOption[] quickItemOptions;//快捷选项栏
    private UI_QuickItemSlot[] quickItemSlots;//快捷栏
    private bool isOpen = false;

    private void Awake()
    {
        skillSlots=GetComponentsInChildren<UI_SkillSlot>(true);

    }

    private void Start()
    {
        quickItemSlots=GetComponentsInChildren<UI_QuickItemSlot>();

        player = FindFirstObjectByType<Player>();
        player.health.OnHealthUpdate += UpdateHealthBar;

        inventory = player.inventory;
        inventory.OnInventoryChange += UpdateQuickSlotsUI;
        inventory.OnQuickSlotUsed += PlayerQuickSlotFeedback;

        //skillSlots=GetComponentsInChildren<UI_SkillSlot>(true);
    }
    //虚拟按钮
    public void PlayerQuickSlotFeedback(int slotNumber) => quickItemSlots[slotNumber].SimulateButtonFeedback();

    //更新快捷栏
    public void UpdateQuickSlotsUI()
    {
        Inventory_Item[] quickItems = inventory.quickItems;//快捷栏的物品
          
        for (int i = 0; i < quickItems.Length; i++)
        {
            quickItemSlots[i].UpdateQuickSlotUI(quickItems[i]);
        }
        //quickItemOptions[slotNumber].UpdateSlot(itemInSlot);
        //quickItemSlots[slotNumber].UpdateQuickSlotUI(itemInSlot);
    }

    //尝试打开或关闭快捷选项栏
    public void TryOpenAndClose(UI_QuickItemSlot quickItemSlot, RectTransform targetRect)
    {
        if (isOpen == false)
        {
            OpenQuickItemOptions(quickItemSlot, targetRect);
        }
        else
        {
            HideQuickItemOptions();
        }
    }

    //打开快捷选项栏
    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot,RectTransform targetRect)
    {
        isOpen = true;
        //获取选项栏
        if (quickItemOptions == null)
            quickItemOptions = quickItemOptionsParent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);
        //寻找所有消耗品
        List<Inventory_Item> consumables = inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Counsumable);
        for (int i = 0; i < quickItemOptions.Length; i++)
        {
            if (i < consumables.Count)
            {
                quickItemOptions[i].gameObject.SetActive(true);
                quickItemOptions[i].SetupOption(quickItemSlot, consumables[i]);
            }
            else
                quickItemOptions[i].gameObject.SetActive(false);
        }

        quickItemOptionsParent.position = targetRect.position + Vector3.up * yoffsetQuickItemParent;
    }

    //隐藏快捷选项栏
    public void HideQuickItemOptions()
    {
        isOpen = false;
        quickItemOptionsParent.position = new Vector3(0, 9999);
    }


    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        foreach(var slot in skillSlots)
        {
            if(slot.skillType == skillType)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        return null;
    }

    private void UpdateHealthBar()
    {
        float currentHealth=Mathf.RoundToInt(player.health.GetCurrentHealth());
        float maxHealth=player.stats.GetMaxHealth();

        float sizeDifference = Mathf.Abs(maxHealth - healthRect.sizeDelta.x);
        if (sizeDifference > 0.1f)
            healthRect.sizeDelta = new Vector2(maxHealth, healthRect.sizeDelta.y);

        healthText.text = currentHealth + "/" + maxHealth;
        healthSlider.value = player.health.GetHealthPercent();
    }
}
