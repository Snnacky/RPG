using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player inventory;

    private UI_CraftPreview craftPreviewUI;

    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButton;


    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.playerInventory;
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
        
        craftPreviewUI = GetComponentInChildren<UI_CraftPreview>();//制作预览ui
        craftPreviewUI.SetupCraftPreview(storage);//初始化
        SetupCraftListButton();
    }

    //设置
    private void SetupCraftListButton()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSlot>(true);//各物品
        craftListButton=GetComponentsInChildren<UI_CraftListButton>(true);//类别按钮
        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var button in craftListButton)
        {
            button.SetCraftSlots(craftSlots);
        }
    }

    //更新craft上的playerInventory
    private void UpdateUI() => inventoryParent.UpdateSlots(inventory.itemList);
}
