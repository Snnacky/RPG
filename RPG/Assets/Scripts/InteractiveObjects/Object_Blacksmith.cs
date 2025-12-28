using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Blacksmith : Object_NPC, IInteractable
{
    private Inventory_Player inventory;//玩家身上的库存
    private Inventory_Storage storage;//储存,铁匠身上


    public void Interact()//玩家靠近铁匠按f触发
    {

        ui.storageUI.SetupStorageUI(storage);//设置ui
        ui.craftUI.SetupCraftUI(storage);

        ui.OpenStorageUI(true);//打开ui
    }

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllToolTips();
        ui.storageUI.gameObject.SetActive(false);
        ui.craftUI.gameObject.SetActive(false);

        ui.OpenStorageUI(false);
    }
}
