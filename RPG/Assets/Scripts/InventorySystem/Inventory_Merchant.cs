using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Merchant :Inventory_Base
{
    private Inventory_Player playerInventory;
    private Inventory_Storage storage;

    [SerializeField] private ItemListDataSO shopData;//所有可售出的物品
    [SerializeField] private int minItemsAmount = 4;


    protected override void Awake()
    {
        base.Awake();
        FillShopList();
        storage = FindAnyObjectByType<Inventory_Storage>(FindObjectsInactive.Include);
    }

    public override void TriggerUpdateUI()
    {
        base.TriggerUpdateUI();
    }

    //买
    public void TryBuyItem(Inventory_Item itemToBuy,bool buyFullStack)
    {
        int amuntToBuy = buyFullStack ? itemToBuy.stackSize : 1;
        for (int i = 0; i < amuntToBuy; i++)
        {
            if(playerInventory.gold<itemToBuy.buyPrice)
            {
                Debug.Log("Not enough gold");
                return;
            }

            if(itemToBuy.itemData.itemType==ItemType.Material)
            {
                playerInventory.storage.AddMaterialToStash(itemToBuy);
            }else
            {
                if(playerInventory.CanAddItem(itemToBuy))
                {
                    var itemToAdd = new Inventory_Item(itemToBuy.itemData);
                    playerInventory.AddItem(itemToAdd);
                }
            }

            playerInventory.gold -= itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);
        }

        TriggerUpdateUI();//触发了的
    }

    public void TrySellItem(Inventory_Item itemToSell,bool sellFullStack,bool isInventorySlot )
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;
        for (int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            playerInventory.gold += sellPrice;
            if (isInventorySlot)
                playerInventory.RemoveOneItem(itemToSell);
            else
                storage.RemoveMaterial(itemToSell);
        }

        TriggerUpdateUI();
    }


    public void FillShopList()
    {
        itemList.Clear();//售卖商店列表
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        //获取可能售卖的物品的数量
        foreach(var itemData in shopData.itemList)
        {
            int randoziedStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack=Mathf.Clamp(randoziedStack,1,itemData.maxStackSizeAtShop);//该物品的数量

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;

            possibleItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemsAmount, maxInventorySize + 1);
        int finalAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);//总的售卖物品种类数量

        for (int i = 0; i < finalAmount; i++)
        {
            var randomIndex = Random.Range(0, possibleItems.Count);
            var item = possibleItems[randomIndex];

            if(CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }
        TriggerUpdateUI();
    }

    public void SetInventory(Inventory_Player inventory)=>this.playerInventory = inventory;
}
