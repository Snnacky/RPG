using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] uiItemSlots;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private UI_ItemSlotParent inventorSlotsParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        inventory=FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    private void UpdateUI()
    {
        UpdateEquipmentSlots();
        inventorSlotsParent.UpdateSlots(inventory.itemList);
    }

    //更新装备栏
    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;//玩家身上的装备列表
        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (playerEquipSlot.HasItem() == false)
                uiEquipSlots[i].UpdateSlot(null);
            else
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }

}
