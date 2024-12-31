using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting.FullSerializer;


public class FileManager
{
    private string dataDirectory;
    private string dataName;


    public void SaveGameData(SaveData data)
    {
        var path = Path.Combine(dataDirectory, dataName);

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        string storedData = JsonUtility.ToJson(data, true);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(storedData);
            }
        }
    }

    public bool IsData()
    {
        var path = Path.Combine(dataDirectory, dataName);
        return File.Exists(path);


    }

    public FileManager(string directory, string name)
    {
        dataDirectory = directory;
        dataName = name;
    }

    public SaveData LoadGameData()
    {
        var path = Path.Combine(dataDirectory, dataName);
        Debug.Log(path);
        SaveData data = null;
        if(File.Exists(path))
        {
            Debug.Log("Save file found at " + path);
            string storedData = "";

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    storedData = reader.ReadToEnd();
                    Debug.Log("JSON Data: " + storedData);
                }
            }
            data = JsonUtility.FromJson<SaveData>(storedData);
            
            
        }
        else
        {
            Debug.Log("Save data not found");
        }
        return data;
    }
}
