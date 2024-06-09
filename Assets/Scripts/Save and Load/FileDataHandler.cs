using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandle
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool encrypt = false;
    private string key = "buitra";
    public FileDataHandle(string _dataDirPath,string _dataFileName,bool _encrypt)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encrypt = _encrypt;
    }
    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(_data, true);
            if(encrypt)
                dataToStore= decryptEncrypt(dataToStore);
            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }catch (Exception ex)
        {
            Debug.Log("error to trying save wiht path : " + fullPath + "\n" + ex);
        }
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        GameData data = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (encrypt)
                    dataToLoad = decryptEncrypt(dataToLoad);
                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.Log("error to trying load wiht path : " + fullPath + "\n" + ex);
            }
        }
        return data;
    }
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
    private string decryptEncrypt(string _data)
    {
        string dataModifies = "";
        for(int i=0;i<_data.Length;i++)
        {
            dataModifies += (char) (_data[i] ^ key[i % key.Length]);
        }
        return dataModifies;
    }
}

