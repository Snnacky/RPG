using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory;
    public List<Inventory_Item> materialStash;

    //制作物品
    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeMaterials(itemToCraft);//消耗材料
        playerInventory.AddItem(itemToCraft);//添加物品
    }

    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }

    //消耗材料
    private void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requiredItem.stackSize;

            amountToConsume -= ConsumedMaterilasAmount(playerInventory.itemList, requiredItem);//消耗玩家身上的

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterilasAmount(itemList, requiredItem);//消耗仓库里面的

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterilasAmount(materialStash, requiredItem);//消耗材料库里面的
        }
    }


    private int ConsumedMaterilasAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;
        List<Inventory_Item> list = new List<Inventory_Item>(itemList);

        foreach(var item in list)
        {
            if (item.itemData != neededItem.itemData)
                continue;

            int removeAmount = Mathf.Min(item.stackSize, amountNeeded - consumedAmount);
            item.stackSize -= removeAmount;
            consumedAmount += removeAmount;

            if(item.stackSize<=0)
                itemList.Remove(item);

            if (consumedAmount >= amountNeeded)
                break;
        }

        return consumedAmount;
    }

    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }
        return true;
    }


    public int GetAvailableAmountOf(ItemDataSO requiredItem)
    {
        int amount = 0;
        foreach(var item in playerInventory.itemList)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach(var item in itemList)
        {
            if(item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach(var item in materialStash)
        {
            if(item.itemData==requiredItem)
                amount += item.stackSize;
        }

        return amount;
    }



    //添加材料物品到储藏
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);
        if (stackableItem != null)
        {
            stackableItem.AddStack();
        }
        else
        {
            var item = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(item);
        }
        
        TriggerUpdateUI();
    }

    public void RemoveMaterial(Inventory_Item itemToRemove)
    {
        Inventory_Item itenInInventory = materialStash.Find(item => item == itemToRemove);
        if (itenInInventory.stackSize > 1)
            itenInInventory.stackSize--;
        else
            materialStash.Remove(itenInInventory);
        TriggerUpdateUI();
    }

    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = materialStash.FindAll(item => item.itemData == itemToAdd.itemData);
        foreach (var stackable in stackableItems)
        {
            if(stackable.CanAddStack())
                return stackable;
        }
        return null;
    }

    public void SetInventory(Inventory_Player inventory) => this.playerInventory = inventory;

    //玩家到库存
    public void FromPlayerToInventory(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItem(item))
            {
                Inventory_Item itemToAdd = new Inventory_Item(item.itemData);
                playerInventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        
        TriggerUpdateUI();
    }
    //库存到玩家
    public void FromInventoryToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (playerInventory.CanAddItem(item))
            {
                Inventory_Item itemToAdd = new Inventory_Item(item.itemData);
                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }

       
        TriggerUpdateUI();
    }
}
