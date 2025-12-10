using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;


    [SerializeField] private UI_ItemSlotParent inventorSlotsParent;
    [SerializeField] private UI_EquipSlotParent uiEquipSlotParent;


    private void Awake()
    {
        inventory=FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    private void UpdateUI()
    {
        uiEquipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        inventorSlotsParent.UpdateSlots(inventory.itemList);
    }



}
