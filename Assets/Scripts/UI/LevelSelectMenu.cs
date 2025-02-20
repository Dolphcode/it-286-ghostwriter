using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Button levelLoadButton;
    [SerializeField]
    private Button levelActivateButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartLevelLoad()
    {
        LevelLoader._Instance.LoadLevel(1);
    }

    public void ActivateLoadedLevel()
    {
        //LevelLoader._Instance.ActivateLevel();
    }
}
