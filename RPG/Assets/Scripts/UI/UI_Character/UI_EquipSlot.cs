using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    public ItemType slotType;

    public string uniqueId;

    private void OnValidate()
    {
        gameObject.name="UI_EquipmentSlot - "+slotType.ToString();

        if(string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        inventory.UnequipItem(itemInSlot);
    }
}
