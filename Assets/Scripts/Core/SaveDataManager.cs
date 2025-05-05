using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager _Instance { get; private set; } = null;

    [Header("Save Path Config")]
    [SerializeField] string dataDirPath = "";
    [SerializeField] string dataFileName = "";

    [Header("Default Data Config")]
    [SerializeField] int numItems;
    [SerializeField] int defaultMoney;

    // Internal save state
    private SaveData data;
    private string fullPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Set the instance
        _Instance = this;
        fullPath = Path.Combine(dataDirPath, dataFileName);
        Debug.Log("Working with " + fullPath);
        if (File.Exists(fullPath))
        {
            Debug.Log("file exists, ripping from path");
            // Get the raw string from the save file
            string dataToLoad;
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }

            // Deserialize data
            data = JsonUtility.FromJson<SaveData>(dataToLoad);

            InputSystem.actions.LoadBindingOverridesFromJson(data.bindings);
        } else {
            Debug.Log("nonexistent, creating the file");

            // Attempt to create the directory for this file
            try
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                

                // Create default data
                data = new SaveData();
                data.playerMoney = defaultMoney;
                data.itemCounts = new int[numItems];
                data.bindings = InputSystem.actions.SaveBindingOverridesAsJson();

                // Create the string for the save data
                string dataToStore = JsonUtility.ToJson(data, true);

                // Write to file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            } catch (System.Exception e)
            {
                Debug.LogError("Error occurred while trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        
        // 
    }

    public void LoadData(LevelDataManager levelData)
    {
        Debug.Log("loading data");
        levelData.AddMoney(data.playerMoney);
        for (int i = 0; i < numItems; ++i)
            for (int j = 0; j < data.itemCounts[i]; ++j)
                levelData.AddItem(i + 1);
    }

    public void SaveData(LevelDataManager levelData)
    {
        data.playerMoney = levelData.GetMoney();
        for (int i = 0; i < numItems; ++i)
        {
            data.itemCounts[i] = levelData.GetOwnedCount(i + 1);
        }
        data.bindings = InputSystem.actions.SaveBindingOverridesAsJson();

        Debug.Log("attempting to save data");
        // Attempt to create the directory for this file
        try
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));


            // Create the string for the save data
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occurred while trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class SaveData
{
    public int     playerMoney;
    public int[]   itemCounts;
    public string   bindings;
}
