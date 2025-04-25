using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas pauseCanvas;

    private bool paused = false;
    public bool IsPaused { get { return paused; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseCanvas.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseCanvas.enabled = true;
        }

        // Check if the paused menu should just not be up at all
        if (LevelLoader._Instance.loadingLevel || 
            SceneManager.GetActiveScene().buildIndex == LevelLoader.MAIN_INDEX)
        {
            paused = false;
            Cursor.lockState = CursorLockMode.None;
            pauseCanvas.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                paused = false;
            } else
            {
                paused = true;
            }
        }
    }
}
