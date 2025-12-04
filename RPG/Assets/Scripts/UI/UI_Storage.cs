using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    [SerializeField] private UI_ItemSlotParent inventoryParent;//Íæ¼Ò¿â´æ¸¸¼¶
    [SerializeField] private UI_ItemSlotParent storageParent;//´æ´¢¸¸¼¶
    [SerializeField] private UI_ItemSlotParent materialStashParent;//²ÄÁÏ´æ´¢¸¸¼¶

    public void SetupStorage(Inventory_Player inventory, Inventory_Storage storage)
    {
        this.inventory = inventory;
        this.storage = storage;
        storage.OnInventoryChange += UpdateUI;

        UI_StorageSlot[] storageSlots=GetComponentsInChildren<UI_StorageSlot>();

        foreach(var slot in storageSlots)
            slot.SetStorage(storage);
        UpdateUI();
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialStash);
    }
}
