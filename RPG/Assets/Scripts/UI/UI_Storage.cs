using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    [SerializeField] private UI_ItemSlotParent inventoryParent;//玩家库存父级
    [SerializeField] private UI_ItemSlotParent storageParent;//存储父级
    [SerializeField] private UI_ItemSlotParent materialStashParent;//材料存储父级

    //初始化
    public void SetupStorageUI( Inventory_Storage storage)
    {
        inventory = storage.playerInventory;
        this.storage = storage;
        storage.OnInventoryChange += UpdateUI;

        UI_StorageSlot[] storageSlots=GetComponentsInChildren<UI_StorageSlot>();

        foreach(var slot in storageSlots)
            slot.SetStorage(storage);//初始化栏

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if(storage==null) return;

        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialStash);
    }
}
