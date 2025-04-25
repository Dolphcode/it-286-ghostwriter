using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager _Instance { get; private set; } = null;
    private void Awake()
    {
        _Instance = this;
    }

    [Header("References")]
    [SerializeField] private Canvas pauseCanvas;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mixerAsset;
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    private bool paused = false;
    public bool IsPaused { get { return paused; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        UpdateAudio();

        if (!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseCanvas.enabled = false;
            Time.timeScale = 1f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseCanvas.enabled = true;
            Time.timeScale = 0f;
        }

        // Check if the paused menu should just not be up at all
        if (LevelLoader._Instance.loadingLevel || 
            SceneManager.GetActiveScene().buildIndex == LevelLoader.MAIN_INDEX)
        {
            paused = false;
            Cursor.lockState = CursorLockMode.None;
            pauseCanvas.enabled = false;
            Time.timeScale = 1f;
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

    private void UpdateAudio()
    {
        mixerAsset.SetFloat("Volume_Master", 20 * Mathf.Log10(masterVolSlider.value));
        mixerAsset.SetFloat("Volume_SFX", 20 * Mathf.Log10(sfxVolSlider.value));
    }
}
