using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Details")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item Effect")]
    public ItemEffectDataSO itemEffect;

    [Header("Craft details")]
    public Inventory_Item[] craftRecipe;

    [Header("Merchant details")]
    [Range(0, 10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;
}
