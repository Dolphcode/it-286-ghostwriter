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

    [SerializeField]
    private int playerMoney = 200;

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

    // ----------------------------------------------
    // INVENTORY CONFIG
    // ----------------------------------------------

    /// <summary>
    /// A wrapper method used to increment the total inventory of a specific type of item
    /// </summary>
    /// <param name="index">Which item type to increment</param>
    public void AddItem(int index)
    {
        itemCounts[index]++;
    }

    /// <summary>
    /// A wrapper method used to decrement the total inventory of a specific type of item
    /// </summary>
    /// <param name="index">Which item type to decrement</param>
    public void RemoveItem(int index)
    {
        if (itemCounts[index] == 0) return;
        itemCounts[index]--;
    }

    /// <summary>
    /// This function is called to add an item to the list of items being brought to the level
    /// </summary>
    /// <param name="index">Which item type to bring</param>
    public void BringItem(int index)
    {
        int totalItems = 0;
        foreach (int count in selectedItemCounts) {
            totalItems += count;
        }
        if (totalItems == maxItems) return;
        if (selectedItemCounts[index] + 1 > itemCounts[index]) return;

        selectedItemCounts[index]++;
    }

    /// <summary>
    /// This function is called to remove an item from the list of items being brought to this level
    /// </summary>
    /// <param name="index">Which item type to leave behind</param>
    public void LeaveItem(int index)
    {
        if (selectedItemCounts[index] == 0) return;
        selectedItemCounts[index]--;
    }

    /// <summary>
    /// Get the number of a specific type of item being brought into the level (the number of
    /// a specific item type that are selected)
    /// </summary>
    /// <param name="index">Which item type to check</param>
    /// <returns>The number of that item that is currently selected to be brought into the level</returns>
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
