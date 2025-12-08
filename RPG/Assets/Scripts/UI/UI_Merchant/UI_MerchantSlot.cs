using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;
    public enum MerchantSlotType { MerchantSlot,PlayerSlot}
    public MerchantSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;
        //Âô
        if(slotType==MerchantSlotType.PlayerSlot)
        {
            bool sellFullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TrySellItem(itemInSlot, sellFullStack);
        }
        //Âò
        else if (slotType==MerchantSlotType.MerchantSlot)
        {
            bool buyFullStack=Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInSlot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }

    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;
}
