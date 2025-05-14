using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlogSaveData", menuName = "Scriptable Objects/BlogSaveData")]
public class BlogSaveData : ScriptableObject
{
    /// <summary>
    /// List of photos taken by the player from both camera and remote camera
    /// </summary>
    [SerializeField]
    private List<CaptureData> photosList = new List<CaptureData>();
    /// <summary>
    /// Current blog save number.
    /// </summary>
    [SerializeField]
    private int blogSaveNum=0;
    /// <summary>
    /// Chosen image's CaptureData.
    /// </summary>
    [SerializeField]
    private CaptureData imageData;
    /// <summary>
    /// Capture Manager.
    /// </summary>
    [SerializeField]
    private CaptureManager captureManager1;
    /// <summary>
    /// Blog UI Manager.
    /// </summary>
    [SerializeField]
    private GameObject UIManager;
    /// <summary>
    /// Level Loadscreen Data.
    /// </summary>
    [SerializeField]
    private LevelLoadscreenData levelData;
    /// <summary>
    /// Ghost Data.
    /// </summary>
    [SerializeField]
    private GhostTypeData ghostData;
    /// <summary>
    /// Blog panel.
    /// </summary>
    [SerializeField]
    private GameObject blogDisplayPanel;
    /// <summary>
    /// Level Data Manager.
    /// </summary>
    [SerializeField]
    private LevelDataManager levelDataManager = LevelDataManager._Instance;
    /// <summary>
    /// Level  Manager.
    /// </summary>
    [SerializeField]
    private LevelManager levelManager1;
    public void Start()
    {
        levelManager1 = FindAnyObjectByType<LevelManager>();
        captureManager1 = levelManager1.GetCaptureManager();
        photosList = captureManager1.GetCaptures();
    }
    // Update is called once per frame
    void Update()
    {
        //activePage.text;
    }
    public LevelLoadscreenData GetLevelLoadscreenData()
    {
        return levelData;
    }
    public float GetTotalScore()
    {
        return blogDisplayPanel.GetComponent<BlogPanel>().GetQuestionScore() + GetImageScore();
    }
    public int GetImageScore()
    {
        return imageData.score;
    }
    public int GetBlogSaveNum()
    {
        return blogSaveNum;
    }
    public void SetGhostData(GhostTypeData type)
    {
        ghostData=type;
    }
    public GhostTypeData GetGhostData()
    {
        return ghostData;
    }
    public List<CaptureData> GetPhotosList()
    {  return photosList; }
}