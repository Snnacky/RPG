using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;//存储
    private UI_CraftPreviewMaterialsSlot[] craftPreviewSlots;

    [Header("Item Preview Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Sprite originalIcon;

    private Coroutine changeButton;
    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewMaterialsSlot>(true);//制作所需材料预览栏

        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }

        itemIcon.sprite = originalIcon;
        itemName.text = "";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("1.选择一个类别");
        stringBuilder.AppendLine("2.选择一个物品");
        stringBuilder.AppendLine("3.点击CRAFT!!!!");
        itemInfo.text = stringBuilder.ToString();
        buttonText.text = "CRAFT";

    }
    //调用在CRAFT按钮按下
    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            if (changeButton != null)
                StopCoroutine(changeButton);
            changeButton = StartCoroutine(ChangeButtonText());
            return;
        }
        //制作
        if (storage.CanCraftItem(itemToCraft))
        {
            storage.CraftItem(itemToCraft);
        }

        UpdateCraftPreviewSlots();
    }


    private IEnumerator ChangeButtonText()
    {
        buttonText.text = "选择一个物品!!!";
        yield return new WaitForSeconds(2);
        buttonText.text = "CRAFT";
    }

    //更新预览ui
    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();
        UpdateCraftPreviewSlots();
    }

    //更新所需材料预览
    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }

        //制作所需材料
        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int avaliableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftPreviewSlots[i].SetupMaterialSlot(requiredItem.itemData, avaliableAmount, requiredAmount);
            craftPreviewSlots[i].gameObject.SetActive(true);
        }
    }
}
