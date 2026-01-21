using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;//ui_inGame

    public List<Inventory_EquipmentSlot> equipList;//持有装备列表
    public Inventory_Storage storage { get; private set; }

    [Header("Quick Item Slots")]
    public Inventory_Item[] quickItems = new Inventory_Item[2];//快捷栏的物品

    [Header("Gold Info")]
    public int gold = 100000;
    protected override void Awake()
    {
        base.Awake();
        storage=FindFirstObjectByType<Inventory_Storage>();
    }

    //设置快捷栏
    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSlot)
    {
        quickItems[slotNumber - 1] = itemToSlot;
        TriggerUpdateUI();
    }
    //尝试使用快捷栏物品
    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;//0/1
        var itemToUse=quickItems[slotNumber];

        if (itemToUse == null) return;

        TryUseItem(itemToUse);

        //使用完物品,替换Inventory里面还有相同的物品
        if(FindItem(itemToUse)==null)
        {
            //如果俩个快捷栏装备同一个消耗品
            var item = quickItems[(slotNumber + 1) % 2];
            if (item == itemToUse)
                quickItems[(slotNumber + 1) % 2] = FindSameItem(itemToUse);

            quickItems[slotNumber] = FindSameItem(itemToUse);
        }
        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);//虚拟按钮效果
    }


    //尝试装备物品
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);//从物品栏里面找到该物品
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

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;

        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach (var item in itemList)
        {
            if(item !=null && item.itemData!=null)
            {
                string saveId = item.itemData.saveID;

                //检查字典是否存在该item
                if (data.inventory.ContainsKey(saveId) == false)
                    data.inventory[saveId] = 0;

                data.inventory[saveId] += item.stackSize;
            }
        }

        foreach(var slot in equipList)
        {
            if (slot.HasItem())
                //data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.equipedId;
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;

        foreach (var item in data.inventory)
        {
            string saveId = item.Key;
            int stackSize = item.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if(itemData==null)
            {
                Debug.LogWarning("Item not found:" + saveId);
                continue;
            }

            
            for(int i=0;i<stackSize;i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        foreach(var entry in data.equipedItems)
        {
            string saveId=entry.Key;
            //ItemType loadedSlotType=entry.Value;
            string equipedId=entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);
            Inventory_Item itemToLoad = new Inventory_Item(itemData);

            var slot = equipList.Find(slot => slot.equipedId == equipedId && slot.HasItem() == false);

            EquipItem(itemToLoad, slot);

            //slot.equipedItem = itemToLoad;
            //slot.equipedItem.AddModifiers(player.stats);
            //slot.equipedItem.AddItemEffect(player);
        }

        TriggerUpdateUI();
    }
}
