using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;//持有装备列表
    public Inventory_Storage storage { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        storage=FindFirstObjectByType<Inventory_Storage>();
    }

    //尝试装备物品
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);//从物品栏里面找到该物品
        var matchingSlot = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);//装备栏中寻找所有类型相同的栏
        if (matchingSlot.Count == 0) return;
        foreach (var slot in matchingSlot)
        {
            //如果该装备栏为空,则装备
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlot[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip , slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }
    //装备
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPercent=player.health.GetHealthPercent();

        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);//添加属性给玩家
        slot.equipedItem.AddItemEffect(player);//添加装备效果

        player.health.SetHealthToPercent(savedHealthPercent);
        RemoveOneItem(itemToEquip);
    }
    //解除装备
    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false & replacingItem == false )
        {
            Debug.Log("No Space");
            return;
        }
        float saveHealthPercent=player.health.GetHealthPercent();

        var slot = equipList.Find(slot => slot.equipedItem == itemToUnequip);
        if (slot != null) slot.equipedItem = null;


        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.SetHealthToPercent(saveHealthPercent);
        AddItem(itemToUnequip);
    }
}
