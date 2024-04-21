using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private readonly string encryptedCodeWord = "iwantpizzailovepizza";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // depending on OS, Get the full path name
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // decrypt data
                dataToLoad = EncryptDecrypt(dataToLoad);

                // deserialize the data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // depending on OS, Get the full path name
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create the directory
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize to json string
            string dataToStore = JsonUtility.ToJson(data, true);

            // encrypt data
            dataToStore = EncryptDecrypt(dataToStore);

            // write to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    // simple XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptedCodeWord[i % encryptedCodeWord.Length]);
        }

        return modifiedData;
    }

    public void DeleteData()
    {
        // depending on OS, Get the full path name
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        string[] saveFile = Directory.GetFiles(Path.GetDirectoryName(fullPath));

        foreach (string file in saveFile)
        {
            File.Delete(file);
        }
    }
}
