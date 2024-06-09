using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private string filePath = "idbfs/buitra9489600jrvfv";
    [SerializeField] private bool encrypt;
    private GameData gameData;
    private List<ISaveManager> saveManagers; 
    private FileDataHandle dataHandler;

    [ContextMenu("Delete save file")]
    public void deleteSavedData()
    {
        //Application.persistentDataPath
        dataHandler = new FileDataHandle(filePath, fileName, encrypt);
        dataHandler.Delete();
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }
    private void Start()
    {
        dataHandler = new FileDataHandle(filePath, fileName, encrypt);
        saveManagers = findAllSaveManagers(); // cho dong nay vao awake khi export game
        loadGame();
    }

    public void newGame()
    {
        gameData = new GameData();
    }
    public void loadGame()
    {
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("no data found");
            newGame();
        }
        foreach(ISaveManager manager in saveManagers)
        {
            manager.loadData(gameData);
        }
    }
    public void saveGame()
    {
        foreach (ISaveManager manager in saveManagers)
        {
            manager.saveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        saveGame();
    }
    private List<ISaveManager> findAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers  = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
    public bool hasSaveData()
    {
        if(dataHandler.Load() == null)
            return false;
        return true;
    }
}
