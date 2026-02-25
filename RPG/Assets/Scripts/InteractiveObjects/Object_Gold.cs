using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Gold : Object_NPC, IInteractable
{
    [SerializeField] private int goldToAdd;
    public void Interact()
    {
        Player player = Player.instance;
        Inventory_Player inventory = player.GetComponent<Inventory_Player>();
        if(inventory)
        {
            inventory.AddGold(goldToAdd);
            Debug.Log("1");
        }
            
    }
}
