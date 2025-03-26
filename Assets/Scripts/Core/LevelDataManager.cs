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
    private int playerMoney = 10000;

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
    // PLAYER DATA
    // ----------------------------------------------

    /// <summary>
    /// Get the amount of money the player has
    /// </summary>
    /// <returns>The player's current amount of money</returns>
    public int GetMoney() { return playerMoney; }

    /// <summary>
    /// Adds an amount of money to the player's total money
    /// </summary>
    /// <param name="amount">The amount to be added</param>
    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    /// <summary>
    /// Removes money from the player's bank without checking if that amount can be spent. Money
    /// is automatically clamped to 0 if it drops below a negative number.
    /// </summary>
    /// <param name="amount">The amount to be removed</param>
    public void RemoveMoney(int amount)
    {
        playerMoney -= amount;
        if (playerMoney < 0) playerMoney = 0;
    }

    /// <summary>
    /// Attempts to consume the player's money. If the amount being spent is larger than the amount
    /// the player has, the player's money will not be consumed and this function will return false.
    /// </summary>
    /// <param name="amount">The amount to decrement from the player's money pool</param>
    /// <returns>Whether the spend attempt was successful or not</returns>
    public bool SpendMoney(int amount)
    {
        if (playerMoney < amount) {
            return false;
        }
        playerMoney -= amount;
        return true;
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
