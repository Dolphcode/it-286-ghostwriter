using UnityEngine;

public class LevelDataManager : MonoBehaviour
{

    [Header("Van Inventory Config")]
    [SerializeField]
    private int itemCount = 0;

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
}
