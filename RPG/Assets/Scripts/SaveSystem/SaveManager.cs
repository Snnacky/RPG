using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "unity-SKy.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);//持久数据路径
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindISaveables();//获取场景里所有ISaveable

        yield return null;
        LoadGame();
    }

    public GameData GetGameData() => gameData;

    //加载游戏里所有数据
    private void LoadGame()
    {
        gameData = dataHandler.LoadData();//加载数据
        if(gameData == null)
        {
            Debug.Log("No save data found,creating new save");
            gameData = new GameData();
            return;
        }

        foreach(var saveable in allSaveables) 
            saveable.LoadData(gameData);
    }
    //保存游戏所有数据

    public void SaveGame()
    {
        //保存所有数据
        foreach(var saveable in allSaveables)
            saveable.SaveData(ref gameData);//ref:确保所有数据写在同一个gameData

        dataHandler.SaveData(gameData);
    }

    [ContextMenu("delete save data")]
    //删掉数据文件
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();

        LoadGame();
    }

    //在游戏退出的时候会调用
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //查询所有ISaveable
    private List<ISaveable> FindISaveables()
    {
        return 
            /*获取场景所有object并且不排序*/
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None)
            .OfType<ISaveable>()/*筛选出类型是ISaveable的*/
            .ToList();
    }
}
