using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    private Inventory_Player playerInventory;
    public List<Inventory_Item> materialStash;

    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
            materialStash.Add(itemToAdd);
        
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
