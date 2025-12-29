using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item List", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

    public ItemDataSO GetItemData(string saveId)
    {
        return itemList.FirstOrDefault(item=>item!=null && item.saveID==saveId);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");//查找所有类型是itemdataso t:type

        itemList = guids
            .Select(guid=> AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            /*根据guid转换成asset路径,再通过asset路径加载对象*/
            .Where(item=>item!=null)/*过滤*/
            .ToArray();

        //标记当前脚本所在的资源已修改，并将更改物理写入硬盘，防止丢失。
        EditorUtility.SetDirty(this);//标记修改
        AssetDatabase.SaveAssets();//保存
    }
#endif
}
