using UnityEngine;
using System.Collections.Generic;

public class LevelDataManager : MonoBehaviour
{

    [Header("Van Inventory")]
    [SerializeField]
    private List<ItemData> itemTemplates;
    [SerializeField]
    private int[] itemCounts;
    [SerializeField]
    private int[] selectedItemCounts;
    [SerializeField]
    private int maxItems = 10;

    [Header("Ghost Init")]
    /// <summary>
    /// The ghost prefab
    /// </summary>
    [SerializeField]
    public GameObject ghostPrefab;


    [SerializeField]
    private GameObject obj;

    public static LevelDataManager _Instance { get; private set; } = null;

    // Initialize the static levelloader instance
    void Awake()
    {
        //DontDestroyOnLoad(this);
        _Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Inventory Config Buttons
    public void BringItem(int index)
    {
        int totalItems = 0;
        foreach (int count in selectedItemCounts) {
            totalItems += count;
        }
        if (totalItems == maxItems) return;

        // TODO also check itemCounts for this item
        selectedItemCounts[index]++;
    }

    public void LeaveItem(int index)
    {
        if (selectedItemCounts[index] == 0) return;
        selectedItemCounts[index]--;
    }

    public int GetItemCount(int index)
    {
        return selectedItemCounts[index];
    }


    // Level Loading Functions
    public AsyncInstantiateOperation InstantiateObjects()
    {
        return InstantiateAsync(obj);
    }

    public List<ItemData> GetSpawnItems()
    {
        List<ItemData> output = new List<ItemData>();
        for (int i = 0; i < selectedItemCounts.Length; i++)
        {
            for (int j = 0; j < selectedItemCounts[i]; j++)
            {
                output.Add(itemTemplates[i]);
            }
            selectedItemCounts[i] = 0;
        }
        return output;
    }
}
