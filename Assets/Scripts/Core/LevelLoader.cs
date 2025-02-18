using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Van Inventory Config")]
    [SerializeField]
    private int itemCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Call this function to start loading a level.
    /// TODO: Add parameter for level config
    /// </summary>
    /// <param name="index">Build index of scene to be loaded</param>
    public void LoadLevel(int index)
    {
        StartCoroutine(LoadScene(index));
    }

    /// <summary>
    /// Load the scene asynchronously
    /// </summary>
    /// <param name="index">Buildi ndex of scene to be loaded</param>
    /// <returns>Coroutine</returns>
    public IEnumerator LoadScene(int index)
    {
        float totalProgress = 0f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            Debug.LogWarning(asyncLoad.progress);
            yield return null;
        }
        totalProgress += asyncLoad.progress;

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(0);
        while (!asyncUnload.isDone)
        {
            Debug.LogWarning(asyncUnload.isDone);
            Debug.LogWarning(asyncUnload.progress + totalProgress);
            yield return null;
        }
    }
}
