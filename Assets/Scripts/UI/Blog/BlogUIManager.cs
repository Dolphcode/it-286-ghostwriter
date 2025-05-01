using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlogUIManager : MonoBehaviour
{
    /// <summary>
    /// Max 3 blog saves, 3 pages each.
    /// </summary>
    [SerializeField]
    private BlogSaveData blogSave1, blogSave2, blogSave3;
    /// <summary>
    /// Max 3 blog saves, 3 pages each.
    /// </summary>
    [SerializeField]
    private BlogSaveData[] blogSaves = new BlogSaveData[3];
    [SerializeField]
    private BlogSaveData newSaveData;
    [SerializeField]
    private Button blogSave1Button, blogSave2Button, blogSave3Button;
    [SerializeField]
    private Button prevButton, nextButton, saveButton;
    [SerializeField]
    private GameObject selectScreen, scorePanel, imageSelectPanel;
    [SerializeField]
    private BlogPanel blogPanel;
    [SerializeField]
    private LevelDataManager levelDataManager = LevelDataManager._Instance;
    [SerializeField]
    private int moneyChange;
    /// <summary>
    /// Indicates which save file the player is on (actual save number = this number; starts from 1)
    /// </summary>
    private int currentSave=1;
    /// <summary>
    /// Indicates which page the player is on (actual page number = this number; starts from 1)
    /// </summary>
    private int currentPage=1;
    void Start()
    {
        blogSave1Button.onClick.AddListener(() => LoadSave(1));
        blogSave2Button.onClick.AddListener(() => LoadSave(2));
        blogSave3Button.onClick.AddListener(() => LoadSave(3));
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        saveButton.onClick.AddListener(CloseBlogPanel);
    }
    void Update()
    {
        if (newSaveData!=null)
            blogPanel.LoadSaveData(newSaveData);
        if (blogSaves != null)
        {
            currentPage = 1;
            /*if (newSaveData.GetPages()==null)
            {
                newSaveData.SetPages(new List<BlogPage> { ScriptableObject.CreateInstance<BlogPage>(), ScriptableObject.CreateInstance<BlogPage>(), ScriptableObject.CreateInstance<BlogPage>() });
            }*/
            foreach (BlogSaveData blogSave in blogSaves)
            {
                if (currentSave==1)
                {
                    if (!AssetDatabase.Contains(blogSave))
                        AssetDatabase.CreateAsset(blogSave, "Assets/BlogSaveData1.asset");
                    AssetDatabase.SaveAssets();
                }
                if (currentSave == 2)
                {
                    if (!AssetDatabase.Contains(blogSave))
                        AssetDatabase.CreateAsset(blogSave, "Assets/BlogSaveData2.asset");
                    AssetDatabase.SaveAssets();
                }
                if (currentSave == 3)
                {
                    if (!AssetDatabase.Contains(blogSave))
                        AssetDatabase.CreateAsset(blogSave, "Assets/BlogSaveData3.asset");
                    AssetDatabase.SaveAssets();
                }
            }
        }
        if (currentPage == 1)
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        else if (currentPage == 2)
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
        else if (currentPage == 3)
        {
            nextButton.gameObject.SetActive(false);
            prevButton.gameObject.SetActive(true);
        }
        //newSaveData.SetPage(currentPage);
    }
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
        }
    }
    public void NextPage()
    {
        if (currentPage < 2)
        {
            currentPage++;
        }
    }
    public int GetPageNum()
    {
        return currentPage;
    }    
    public void CloseBlogPanel()
    {
        blogPanel.gameObject.SetActive(false);
    }
    public void OpenBlogPanel()
    {
        blogPanel.gameObject.SetActive(true);

    }
    public void CloseScorePanel()
    {
        scorePanel.SetActive(false);
    }
    public void OpenScorePanel()
    {
        scorePanel.SetActive(true);

    }
    public void CloseSelectScreen()
    {
        selectScreen.SetActive(false);
    }
    public void OpenSelectScreen()
    {
        selectScreen.SetActive(true);
    }
    public void CreateBlogSave(int i)
    {
        newSaveData = ScriptableObject.CreateInstance<BlogSaveData>();
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
    /// <summary>
    /// param int i is the actual number (not index)
    /// </summary>
    /// <param name="i"></param>
    public void RemoveBlogSave(int i)
    {
        blogSaves[i-1] = null;
    }
    public void Publish(int i)
    {
        //some code to save prev published blogs?
        RemoveBlogSave(i);
    }
    public void LoadSave(int num)
    {
        currentSave = num;
        int saveNum = num - 1;
        currentPage = 1;
        if (blogSaves[saveNum] == null)
        {
            CreateBlogSave(saveNum);
            newSaveData = null;
        }
        else
        {
            //sets current save
            newSaveData=blogSaves[saveNum];
        }
    }
    public void SaveFile()
    {
        blogSaves[currentSave-1] = newSaveData;
        newSaveData = null;
    }
    public void AdjustMoney()
    {
        //assuming player averages around 4/10 on each image and gets half of the sentence ques right
        if (blogSaves[currentSave - 1].GetTotalScore() > 20)
        {
            moneyChange = 1000;
            levelDataManager.AddMoney(moneyChange);
        }
        else if (blogSaves[currentSave - 1].GetTotalScore() > 18)
        {
            moneyChange = 500;
        }
        else if (blogSaves[currentSave - 1].GetTotalScore() >= 15)
        {
            moneyChange = 0;
        }
        else if (blogSaves[currentSave - 1].GetTotalScore() < 15)
        {
            moneyChange = -500;
            levelDataManager.RemoveMoney(500);
        }
        else if (blogSaves[currentSave - 1].GetTotalScore() < 10)
        {
            moneyChange = -1000;
            levelDataManager.RemoveMoney(1000);
        }
    }
    public void AdjustCredibility()
    {
        //maybe give shop discount idfk
    }
    public int GetMoneyChange()
    {
        return moneyChange;
    }
}