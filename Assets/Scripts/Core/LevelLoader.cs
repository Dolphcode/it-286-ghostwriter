using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Static class for bootstrapping core game systems/managers
/// </summary>
public static class PerformBootstrap
{
    const string SceneName = "Bootstrapped Scene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        // Avoid loading the bootstrap scene twice
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == SceneName)
            {
                return;
            }
        }

        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }

}

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader _Instance { get; private set; } = null;

    private AsyncOperation asyncLoad = null;

    [Header("UI References")]
    [SerializeField]
    private Canvas levelLoadingScreen;
    [SerializeField]
    private TextMeshProUGUI loadingNumber;
    [SerializeField]
    private TextMeshProUGUI loadingDescriptionLabel;
    [SerializeField]
    private TextMeshProUGUI levelNameLabel;
    [SerializeField]
    private TextMeshProUGUI readyLabel;
    [SerializeField]
    private Image backgroundImage;

    [Header("Loading Data")]
    [SerializeField]
    private List<LevelLoadscreenData> loadscreenDataList;

    public bool loadingLevel { get; private set; } = false;
    public bool levelReady { get; private set; } = false;

    private float progress = 0f;

    // Initialize the static levelloader instance
    void Awake()
    {
        //DontDestroyOnLoad(this);
        _Instance = this;
    }

    private void Start()
    {
        levelLoadingScreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        loadingNumber.text = (Mathf.Round(progress * 100)).ToString() + "%";

        if (Input.GetKeyDown(KeyCode.Space) && levelReady == true)
        {
            ActivateLevel();
        }
    }

    /// <summary>
    /// Call this function to start loading a level.
    /// TODO: Add parameter for level config
    /// </summary>
    /// <param name="index">Build index of scene to be loaded</param>
    public void LoadLevel(int index)
    {
        progress = 0f;
        levelLoadingScreen.enabled = true;

        LevelLoadscreenData screenData = loadscreenDataList[index];
        loadingDescriptionLabel.text = screenData.loadingDescription;
        levelNameLabel.text = screenData.levelName;
        backgroundImage.sprite = screenData.backgroundImage;
        readyLabel.text = "";
        StartCoroutine(LoadScene(index));
    }

    /// <summary>
    /// After instructing the level loader to load a level, 
    /// activate it.
    /// </summary>
    public void ActivateLevel()
    {
        Time.timeScale = 1f;
        levelLoadingScreen.enabled = false;
    }

    /// <summary>
    /// Load the scene asynchronously
    /// </summary>
    /// <param name="index">Buildi ndex of scene to be loaded</param>
    /// <returns>Coroutine</returns>
    public IEnumerator LoadScene(int index)
    {
        loadingLevel = true;
        levelReady = false;
        Time.timeScale = 0f;
        // Load level
        asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        float load_progress = 0f;
        float unload_progress = 0f;
        float item_progress = 0f;
        while (!asyncLoad.isDone) {
            progress = asyncLoad.progress * 0.1f;
            yield return null;
        }
        load_progress = asyncLoad.progress;

        // Unload main menu
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(0);

        while (!asyncUnload.isDone)
        {
            progress = 0.1f * load_progress + 0.1f * unload_progress;
            yield return null;
        }
        unload_progress = asyncUnload.progress;

        // Get the level manager
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();

        // Instantiate the items
        List<ItemData> itemsToSpawn = LevelDataManager._Instance.GetSpawnItems();
        float itemProgressSegments = 1.0f * itemsToSpawn.Count; // Need to instantiate data and behavior
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            ItemData data = Instantiate(itemsToSpawn[i]); // Unfortunately cannot instantiate this asynchronously :(

            AsyncInstantiateOperation<GameObject> obj_instantiation = InstantiateAsync(data.Item);
            while (!obj_instantiation.isDone)
            {
                progress = load_progress * 0.1f + unload_progress * 0.1f + item_progress / itemProgressSegments * 0.7f + obj_instantiation.progress / itemProgressSegments * 0.7f;
                yield return null;
            }
            item_progress += obj_instantiation.progress;
            ItemBehavior behavior = obj_instantiation.Result[0].GetComponent<ItemBehavior>();

            // Assign the behavior to the data and vice versa
            behavior.data = data;
            data.Behavior = behavior;

            levelManager.AddItemToWorld(behavior, i);
        }

        // Now Instantiate the ghost
        AsyncInstantiateOperation<GameObject> ghost_instantiation = InstantiateAsync(LevelDataManager._Instance.ghostPrefab);
        while (!ghost_instantiation.isDone)
        {
            progress = load_progress * 0.1f + unload_progress * 0.1f + item_progress * 0.7f + ghost_instantiation.progress * 0.1f;
            yield return null;
        }
        levelManager.AddGhostToWorld(ghost_instantiation.Result[0].GetComponent<Ghost>());

        progress = 1f;
        levelReady = true;

        readyLabel.text = "Press SPACE to start";
    }
}
