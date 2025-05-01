using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "BlogSaveData", menuName = "Scriptable Objects/BlogSaveData")]
public class BlogSaveData : ScriptableObject
{
    /// <summary>
    /// List of photos taken by the player from both camera and remote camera
    /// </summary>
    //[SerializeField]
    //private List<CaptureData> photosList;
    /// <summary>
    /// List of each ghost in level's data
    /// </summary>
    //[SerializeField]
    //private List<GhostTypeData> ghostDataList;
    [SerializeField]
    private List<GhostTypeData> ghostDataList;
    /// <summary>
    /// List of correct answers for captions
    /// </summary>
    [SerializeField]
    private List<object> answers = new List<object>();
    /// <summary>
    /// List of given answers for captions
    /// </summary>
    [SerializeField]
    private List<object> prompts = new List<object>();
    /// <summary>
    /// List of pages.
    /// </summary>
    [SerializeField]
    private BlogPage page;
    /// <summary>
    /// Current blog save number.
    /// </summary>
    [SerializeField]
    private int blogSaveNum;
    /// <summary>
    /// Current page.
    /// </summary>
    [SerializeField]
    private BlogPage activePage;
    /// <summary>
    /// Total score with just images.
    /// </summary>
    [SerializeField]
    private int totalImageScore = 0;
    /// <summary>
    /// Capture Manager.
    /// </summary>
    //[SerializeField]
    //private CaptureManager captureManager;
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
    /// List of tags for blog.
    /// </summary>
    [SerializeField]
    private List<string> tags;
    [SerializeField]
    private GameObject blogDisplayPanel;
    [SerializeField]
    private LevelDataManager levelDataManager = LevelDataManager._Instance;
    public void Awake()
    {
        //ghostDataList.Add(levelDataManager.ghostPrefab.Get); - function to get type from prefab?
        if (ghostDataList != null)
        {
            int i = 0;
            answers.Add(ghostDataList[i].typeName);
            answers.Add(ghostDataList[i].maxEMF);
            answers.Add(ghostDataList[i].adjustableSpeed);
            for (int c = 0; ghostDataList[i].interactableTypes.Count >= c; c++)
            {
                answers.Add(ghostDataList[i].interactableTypes[c].ToString());
                //should return game.GhostInteractables.DoorInteractable or DoorInteractable?
            }
        }
        

       //need to find a way to access which interactables happened instead of possible interaction types
    }
    // Update is called once per frame
    void Update()
    {
        //activePage.text;
    }
    public BlogPage GetPage()
    {
        return page;
    }
    public List<object> GetAnswers()
    {
        return answers;
    }
    public int GetPageNum()
    {
        return UIManager.GetComponent<BlogUIManager>().GetPageNum();
    }
    /*public void SetPhotoList()
    {
        for (int i = 0; i < captureManager.CaptureCount; i++)
        {
            photosList.Add(captureManager.GetCaptureData(i));
        }
    }*/
    public LevelLoadscreenData GetLevelLoadscreenData()
    {
        return levelData;
    }
    public void Publish()
    {
        // removes blog save from manager's blog save list
        //UIManager.RemoveBlogSave(UIManager.GetBlogSaves()[blogSaveNum]);
    }
    public float GetTotalScore()
    {
        return blogDisplayPanel.GetComponent<BlogPanel>().GetSentenceScore() + GetImageScore();
    }
    public int GetImageScore()
    {
        totalImageScore += page.imageData.score;
        return totalImageScore;
    }
    public List<string> GetTags()
    {
        return tags;
    }
    public void AddTag(string tag)
    {
        tags.Add(tag);
    }
    public int GetBlogSaveNum()
    {
        return blogSaveNum;
    }
}