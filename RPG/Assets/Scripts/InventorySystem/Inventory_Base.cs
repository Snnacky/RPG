using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    protected Player player;
    public event Action OnInventoryChange;//事件触发在ui_Inventory,ui_PlayerStats,UI_Merchant

    public int maxInventorySize = 10;//最大持有物品数量
    public List<Inventory_Item> itemList = new List<Inventory_Item>();//持有物品列表

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    //使用物品
    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);
        if (consumable == null) return;

        if (consumable.itemEffectData.CanBeUse(player) == false) return;


        consumable.itemEffectData.ExecuteEffect();//物品效果

        if(consumable.stackSize>1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);
        TriggerUpdateUI();
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackable(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);
        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        
        return null;
    }
    //添加物品
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindStackable(itemToAdd);//查找UI栏里面是否已经有相同的物品
        if (itemInInventory != null )//如果有并且没有超过存放范围
        {
            itemInInventory.AddStack();//数量增1
        }
        else
        {
            //新添加一个栏
            itemList.Add(itemToAdd);
        }
        TriggerUpdateUI();//触发事件,更新ui
    }
    //移除物品
    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itenInInventory = itemList.Find(item => item == itemToRemove);
        Debug.Log("3");
        if (itenInInventory.stackSize > 1)
            itenInInventory.stackSize--;
        else
            itemList.Remove(itenInInventory);
        TriggerUpdateUI();
    }

    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        for (int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }

    //寻找同一个东西
    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }
    //寻找同一个itemdata的东西
    public Inventory_Item FindSameItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item=>item.itemData==itemToFind.itemData);
    }

    public virtual void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
