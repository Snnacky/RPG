using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;
    public enum MerchantSlotType { MerchantSlot, PlayerSlot, MaterialSlot }
    public MerchantSlotType slotType;


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (itemInSlot == null)
            return;

        //卖
        if (slotType == MerchantSlotType.PlayerSlot || slotType == MerchantSlotType.MaterialSlot)
        {
            bool sellFullStack = Input.GetKey(KeyCode.LeftControl);
            Debug.Log("1");
            merchant.TrySellItem(itemInSlot, sellFullStack,slotType==MerchantSlotType.PlayerSlot);
        }
        //买
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInSlot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        //商品
        if (slotType == MerchantSlotType.MerchantSlot)
            ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, true, true, true);
        //要卖的
        else
            ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, false, true, false);
    }

    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;

}
