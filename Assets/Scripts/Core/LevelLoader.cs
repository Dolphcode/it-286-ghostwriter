using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Canvas levelLoadingScreen;

    [SerializeField]
    private TextMeshProUGUI loadingNumber;

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
        
        loadingNumber.text = (progress / 3.0f).ToString();
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
        StartCoroutine(LoadScene(index));
    }

    /// <summary>
    /// After instructing the level loader to load a level, 
    /// activate it.
    /// </summary>
    public void ActivateLevel()
    {
        if (asyncLoad != null && levelReady == true)
        {
            loadingLevel = false;
            levelReady = false;
            
            asyncLoad = null;
        }
    }

    /// <summary>
    /// Load the scene asynchronously
    /// </summary>
    /// <param name="index">Buildi ndex of scene to be loaded</param>
    /// <returns>Coroutine</returns>
    public IEnumerator LoadScene(int index)
    {
        loadingLevel = true;

        // Load level
        asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        float total_progress = 0f;
        while (!asyncLoad.isDone) {
            progress = asyncLoad.progress;
            yield return null;
        }

        total_progress = progress;

        // Unload main menu
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(0);

        while (!asyncUnload.isDone)
        {
            progress = total_progress + asyncUnload.progress;
            yield return null;
        }

        total_progress = progress;

        // Instantiate objects
        for (int i = 0; i < 0; i++)
        {
            AsyncInstantiateOperation instantiation = LevelDataManager._Instance.InstantiateObjects();

            while (!instantiation.isDone)
            {
                progress = total_progress + (instantiation.progress / 1000f);
                yield return null;
            }
            total_progress += progress / 1000f;
        }

        levelLoadingScreen.enabled = false;
    }
}
