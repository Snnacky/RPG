using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Inventory_Item itemInSlot {  get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;
    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    [SerializeField] private bool canPointerDown = true;

    protected void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui=GetComponentInParent<UI>();
        inventory=FindFirstObjectByType<Inventory_Player>();
    }
    //鼠标点击
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || canPointerDown == false)
            return;

        if (itemInSlot.itemData.itemType==ItemType.Counsumable)
        {
            if(itemInSlot.itemEffectData.CanBeUse()==false) return;
            inventory.TryUseItem(itemInSlot);
        }
        else
            inventory.TryEquipItem(itemInSlot);
        ui.itemToolTip.ShowToolTip(false, null);
    }

    //更新栏
    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;
        if(itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            itemStackSize.text = "";
            return;
        }

        Color color = Color.white;color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite=itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
