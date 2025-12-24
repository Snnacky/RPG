using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO dropData;

    [Header("Drop restrctions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 3;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
            DropItems();
    }

    //掉落物品
    public virtual void DropItems()
    {
        if(dropData == null)
        {
            Debug.Log("Not DropData");
            return;
        }

        List<ItemDataSO> itemToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemToDrop[i]);
        }
    }

    //生成掉落物
    protected void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab,transform.position,Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    //选择要掉落的物品
    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops=new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();
        float maxRarityAmount = this.maxRarityAmount;

        //初步筛选
        foreach(var item in dropData.itemList)
        {
            float dropChance=item.GetDropChance();

            if(Random.Range(0,100)<=dropChance)
                possibleDrops.Add(item);
        }
        //按稀有度进行排序,从大到小
        possibleDrops=possibleDrops.OrderByDescending(item =>item.itemRarity).ToList();
        //选出最终掉落物
        foreach (var item in possibleDrops)
        {
            if(maxRarityAmount>item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount -= item.itemRarity;
            }
        }

        return finalDrops;
    }
}
