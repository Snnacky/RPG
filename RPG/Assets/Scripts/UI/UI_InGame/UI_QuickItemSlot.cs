using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickItemSlot : UI_ItemSlot
{
    private Button button;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int slotNumber;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    //设置快捷栏
    public void SetupQuickSlotItem(Inventory_Item itemToPass)
    {
        inventory.SetQuickItemInSlot(slotNumber, itemToPass);//设置快捷栏
    }

    //虚拟按钮按下
    public void SimulateButtonFeedback()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }


    //更新快捷栏
    public void UpdateQuickSlotUI(Inventory_Item currentItemInSlot)
    {
        if (currentItemInSlot == null || currentItemInSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStackSize.text = "";
            return;
        }
        itemIcon.sprite = currentItemInSlot.itemData.itemIcon;
        itemStackSize.text = currentItemInSlot.stackSize.ToString();

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.inGameUI.TryOpenAndClose(this, rect);
        //ui.inGameUI.OpenQuickItemOptions(this, rect);
    }
}
