using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Blacksmith : Object_NPC, IInteractable
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;


    public void Interact()//玩家靠近铁匠铺按f触发
    {
        ui.storageUI.SetupStorage(inventory, storage);//设置ui
        ui.storageUI.gameObject.SetActive(!ui.storageUI.gameObject.activeSelf);

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
        ui.SwithoffAllToolTips();
        ui.storageUI.gameObject.SetActive(false);
    }
}
