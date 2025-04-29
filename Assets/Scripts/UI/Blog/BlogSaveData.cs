using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlogSaveData", menuName = "Scriptable Objects/BlogSaveData")]
public class BlogSaveData : ScriptableObject
{
    /// <summary>
    /// List of photos taken by the player from both camera and remote camera
    /// </summary>
    [SerializeField]
    private List<CaptureData> photosList;
    /// <summary>
    /// List of each ghost in level's data
    /// </summary>
    [SerializeField]
    private List<GhostTypeData> ghostDataList;
    /// <summary>
    /// List of correct answers for captions
    /// </summary>
    [SerializeField]
    private List<string> answers;
    /// <summary>
    /// List of given answers for captions
    /// </summary>
    [SerializeField]
    private List<object> prompts;
    /// <summary>
    /// List of pages.
    /// </summary>
    [SerializeField]
    private List<BlogPage> pages;
    /// <summary>
    /// Current blog save number.
    /// </summary>
    [SerializeField]
    private int blogSaveNum;
    /// <summary>
    /// List of pages.
    /// </summary>
    [SerializeField]
    private BlogPage blogPage1, blogPage2, blogPage3;
    /// <summary>
    /// Current page.
    /// </summary>
    [SerializeField]
    private BlogPage activePage;
    /// <summary>
    /// List of given answers for captions
    /// </summary>
    [SerializeField]
    private int currentScore;
    /// <summary>
    /// Capture Manager.
    /// </summary>
    [SerializeField]
    private CaptureManager captureManager;
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
    void OnEnable()
    {
        SetPhotoList();
        for (int i = 0; ghostDataList.Count >= i; i++)
        {
            answers.Add(ghostDataList[i].typeName);
            answers.Add(ghostDataList[i].maxEMF.ToString());
            answers.Add(ghostDataList[i].maxSpeed.ToString());
            answers.Add(ghostDataList[i].adjustableSpeed.ToString());
            for (int c = 0; ghostDataList[i].interactableTypes.Count >= c; c++)
            {
                answers.Add(ghostDataList[i].interactableTypes[c].ToString());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        activePage.text;
    }
    public List<BlogPage> GetPages()
    {
        return pages;
    }
    public BlogPage GetPage()
    {
        return activePage;
    }
    public void SetPage(int pageNum)
    {
        activePage = pages[pageNum-1];
    }
    public int GetPageNum(BlogPage pageInput)
    {
        return pageInput.GetPageNum();
    }
    public void SetPhotoList()
    {
        for (int i = 0; i < captureManager.CaptureCount; i++)
        {
            photosList.Add(captureManager.GetCaptureData(i));
        }
    }
    public LevelLoadscreenData GetLevelLoadscreenData()
    {
        return levelData;
    }
    public void Publish()
    {
        // removes blog save from manager's blog save list
        UIManager.RemoveBlogSave(UIManager.GetBlogSaves()[blogSaveNum]);
    }
    public void DisplayPage(BlogPage page)
    {
        
    }
    public List<string> GetTags()
    {
        return tags;
    }
}