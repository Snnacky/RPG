using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType { StorageSlot,PlayInventorySlot}
    public StorageSlotType slotType;
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        if (itemInSlot.itemData.itemType == ItemType.Material) return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        if(slotType==StorageSlotType.StorageSlot)
            storage.FromInventoryToPlayer(itemInSlot, transferFullStack);
        
        if(slotType == StorageSlotType.PlayInventorySlot)
            storage.FromPlayerToInventory(itemInSlot, transferFullStack);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
