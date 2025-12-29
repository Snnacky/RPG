using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;//文件路径
    private bool encripyData;
    private string codeWord = "Snnacky";

    public FileDataHandler(string dataDirPath,string dataFileName,bool encryptData)
    {
        fullPath=Path.Combine(dataDirPath,dataFileName);
        this.encripyData = encryptData;
    }

    //保存数据
    public void SaveData(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//新建目录

            string dataToSave = JsonUtility.ToJson(gameData, true);//转换成json字符串
            
            //加密
            if(encripyData)
                dataToSave=EncryptDecrypt(dataToSave);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("error on trying t save data to file" + fullPath + "\n" + e);
        }
    }
    
    //加载数据
    public GameData LoadData()
    {
        GameData loadData = null;

        //检查是否存在保存文件
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                //打开该文件
                using(FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    //读文件
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();//读取所有字符
                    }
                }
                //解密
                if (encripyData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                //从json转换成gameData
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("error on trying to load data from file:" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    //删除数文件
    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    //加密  异或加密算法
    private string EncryptDecrypt(string data)
    {
        string modifedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifedData;
    }
}
