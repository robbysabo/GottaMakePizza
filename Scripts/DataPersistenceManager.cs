using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    /* File Location: C:\Users\USERNAME\AppData\LocalLow\Robert Sabo\Gotta Make Pizza*/
    private string fileName = "data.save";

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindALlDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // load any saved data
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize new game.
        if (this.gameData == null)
        {
            Debug.Log("No Data found. Create new Save.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence d in dataPersistenceObjects)
        {
            d.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence d in dataPersistenceObjects)
        {
            d.SaveData(ref gameData);
        }

        // save the data
        dataHandler.Save(gameData);
    }

    public void DeleteData()
    {
        dataHandler.DeleteData();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindALlDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
