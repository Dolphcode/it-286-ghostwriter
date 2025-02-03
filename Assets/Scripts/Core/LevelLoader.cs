using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time);
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone) {
            Debug.LogWarning(asyncLoad.progress.ToString());
            yield return null;
        }
    }
}
