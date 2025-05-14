using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlogUIManager : MonoBehaviour
{
    [SerializeField]
    private BlogSaveData[] blogSaves = new BlogSaveData[3];
    [SerializeField]
    private BlogSaveData newSaveData;
    [SerializeField]
    private Button blogSave1Button, blogSave2Button, blogSave3Button;
    [SerializeField]
    private Button saveButton, publishButton;
    [SerializeField]
    private GameObject selectScreen, scorePanel;
    [SerializeField]
    private BlogPanel blogPanel;
    [SerializeField]
    private LevelDataManager levelDataManager = LevelDataManager._Instance;
    [SerializeField]
    private int moneyChange;
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private List<Ghost> ghostList;
    /// <summary>
    /// Indicates which save file the player is on (actual save number = this number; starts from 1)
    /// </summary>
    [SerializeField]
    private int currentSave=1;
    [SerializeField]
    private LevelManager levelManager;
    void Start()
    {
        if (FindAnyObjectByType<LevelManager>() != null)
            levelManager = FindAnyObjectByType<LevelManager>();
        blogSave1Button.onClick.AddListener(() => LoadSave(1));
        blogSave2Button.onClick.AddListener(() => LoadSave(2));
        blogSave3Button.onClick.AddListener(() => LoadSave(3));
    }
    void Update()
    {
        if (newSaveData!=null&& blogPanel!=null)
            blogPanel.LoadSaveData(newSaveData);
        if (FindAnyObjectByType<LevelManager>() != null)
            levelManager = FindAnyObjectByType<LevelManager>();
        if (FindAnyObjectByType<LevelManager>() != null&&levelManager.GetGhostList() != null)
        {
            ghostList = levelManager.GetGhostList();
        }
    }
    public void CloseBlogPanel()
    {
        blogPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Given blog save number i and GhostTypeData type, creates a save.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="type"></param>
    public void CreateBlogSave(int i, GhostTypeData type)
    {
        newSaveData = ScriptableObject.CreateInstance<BlogSaveData>();
        newSaveData.SetGhostData(type);
        blogSaves[i-1]= newSaveData;
    }
    public BlogSaveData[] GetBlogSaves()
    {
        return blogSaves;
    }
    public void RemoveBlogSave(BlogSaveData remove)
    {
        for(int i=0;blogSaves.Length>i;i++)
        {
            if (blogSaves[i] == remove)
                blogSaves[i] = null;
        }
    }
    public void Publish(int i)
    {
        blogSaves[i - 1] = null;
    }
    public void LoadSave(int num)
    {

        currentSave = num;
        int saveNum = num - 1;
        if (blogSaves[saveNum] == null)
        {
            CreateBlogSave(num, levelManager.GetGhostList()[0].GetGhostTypeData());
        }
        else
        {
            newSaveData = blogSaves[saveNum];
        }
    }
    public void SaveFile()
    {
        blogSaves[currentSave-1] = newSaveData;
        newSaveData = null;
    }
    public void AdjustMoney(int score)
    {
        if (score > 18)
        {
            moneyChange = 1000;
            levelDataManager.AddMoney(moneyChange);
            shopManager.DiscountShop(100);
        }
        else if (score > 15)
        {
            moneyChange = 700;
            levelDataManager.AddMoney(moneyChange);
            shopManager.DiscountShop(50);
        }
        else if (score >= 8)
        {
            moneyChange = 0;
        }
        else if (score < 8)
        {
            moneyChange = -500;
            levelDataManager.RemoveMoney(-1 * moneyChange);
        }
        else if (score < 6)
        {
            moneyChange = -1000;
            levelDataManager.RemoveMoney(-1 * moneyChange);
        }
    }
    public int GetMoneyChange()
    {
        return moneyChange;
    }
}