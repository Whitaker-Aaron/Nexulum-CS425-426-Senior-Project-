using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    SaveData data;
    public bool hasData;
    List<SaveSystemInterface> allSaveObjects;
    FileManager fileManager;
    LifetimeManager lifetimeManager;
    private string fileName = "nexulumSaveData.game";
    // Start is called before the first frame update
    void Awake()
    {
        var saveObjects = FindObjectsOfType<MonoBehaviour>().OfType<SaveSystemInterface>();
        allSaveObjects = new List<SaveSystemInterface>(saveObjects);
        fileManager = new FileManager(Application.persistentDataPath, fileName);
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        hasData = fileManager.IsData();
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name != "TitleScreen")
        {
            Debug.Log("Not starting from title screen");
            NewGame();
            lifetimeManager.InitializeManagers();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        foreach (var item in allSaveObjects)
        {
            item.SaveData(ref data);
        }
        Debug.Log("Saved");
        Debug.Log("Saved health: " + data.playerHealth);
        fileManager.SaveGameData(data);
    }

    public void LoadGame()
    {
        data = fileManager.LoadGameData();

        foreach (var item in allSaveObjects)
        {
            item.LoadData(data);
        }
        
    }

    public void NewGame()
    {
        data = new SaveData();

        foreach (var item in allSaveObjects)
        {
            item.LoadData(data);
        }

    }
}
