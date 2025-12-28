using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;

    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot,Inventory_Item itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);//设置快捷栏
        ui.inGameUI.HideQuickItemOptions();//隐藏快捷选项栏
    }
}
