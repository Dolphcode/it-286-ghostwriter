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
        }
        return output;
    }
}
