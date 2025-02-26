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

    public void StartLevelLoad(int index)
    {
        LevelLoader._Instance.LoadLevel(index);
    }

    public void ActivateLoadedLevel()
    {
        //LevelLoader._Instance.ActivateLevel();
    }
}
