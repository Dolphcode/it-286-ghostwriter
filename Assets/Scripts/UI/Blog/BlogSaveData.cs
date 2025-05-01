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
    private List<BlogPage> pages = new List<BlogPage>();
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
        if (pages==null)
        {
            pages.Add(ScriptableObject.CreateInstance<BlogPage>());
            pages.Add(ScriptableObject.CreateInstance<BlogPage>());
            pages.Add(ScriptableObject.CreateInstance<BlogPage>());
        }
        for (int i = 0; ghostDataList.Count > i; i++)
        {
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
    public List<BlogPage> GetPages()
    {
        return pages;
    }
    public BlogPage GetPage()
    {
        return activePage;
    }
    public BlogPage GetPage(int i)
    {
        return pages[i-1];
    }
    public List<object> GetAnswers()
    {
        return answers;
    }
    /// <summary>
    /// Which page is being edited at a time
    /// </summary>
    /// <param name="pageNum"></param>
    public void SetPage(int pageNum)
    {
        if (pageNum>0)
            activePage = pages[pageNum-1];
    }
    public void SetPages(List<BlogPage> x)
    {
        pages = x;
    }
    /// <summary>
    /// Save page data.
    /// </summary>
    /// <param name="pageNum"></param>
    public void SavePage(int pageNum)
    {
        pages[pageNum - 1] = activePage;
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
        foreach (BlogPage b in pages)
        {
            totalImageScore += b.imageData.score;
        }
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