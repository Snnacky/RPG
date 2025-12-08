using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemDataToCraft;

    [SerializeField] private UI_CraftPreview craftPreview;
    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;

    //设置按钮
    public void SetupButton(ItemDataSO craftData)
    {
        this.itemDataToCraft = craftData;

        craftItemIcon.sprite = craftData.itemIcon;
        craftItemName.text = craftData.itemName;
    }

    //调用在按钮触发
    public void UpdateCraftPreview()=>craftPreview.UpdateCraftPreview(itemDataToCraft);//设置武器锻造预制
}
