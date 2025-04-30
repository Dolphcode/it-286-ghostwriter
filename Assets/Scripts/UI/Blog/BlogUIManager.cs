using System.Collections.Generic;
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
    private List<BlogSaveData> blogSaves;
    [SerializeField]
    private BlogSaveData newSaveData;
    [SerializeField]
    private Button blogSave1Button, blogSave2Button, blogSave3Button;
    [SerializeField]
    private Button prevButton, nextButton, saveButton;
    [SerializeField]
    private GameObject blogPanel, selectScreen, scorePanel, imageSelectPanel;
    [SerializeField]
    private LevelDataManager levelDataManager = LevelDataManager._Instance;
    [SerializeField]
    private int moneyChange;
    /// <summary>
    /// Indicates which save file the player is on.
    /// </summary>
    private int currentSave;
    /// <summary>
    /// Indicates which page the player is on.
    /// </summary>
    private int currentPage=0;
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
        if (blogSaves != null)
        {
            currentPage = 1;
        }
        if (currentPage == 1)
        {
            prevButton.gameObject.SetActive(false);
            newSaveData.SetPage(currentPage);
        }
        else if (currentPage == 2)
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            newSaveData.SetPage(currentPage);
        }
        else if (currentPage == 3)
        {
            nextButton.gameObject.SetActive(false);
            newSaveData.SetPage(currentPage);
        }
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
        blogPanel.SetActive(false);
    }
    public void OpenBlogPanel()
    {
        blogPanel.SetActive(true);

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
    public void SaveBlogData(int saveNum)
    {
        blogSaves.Add(newSaveData);
    }
    public void CreateBlogSave(int i)
    {
        blogSaves[i-1]= newSaveData;
    }
    public List<BlogSaveData> GetBlogSaves()
    {
        return blogSaves;
    }
    public void RemoveBlogSave(BlogSaveData remove)
    {
        blogSaves.Remove(remove);
    }
    public void Publish(int i)
    {
        int x = i - 1;
        blogSaves[x] = null;
        blogSaves.Remove(blogSaves[currentSave-1]);
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
            DisplaySave(num);
        }
        else
        {
            //sets current save
            newSaveData=blogSaves[saveNum];
            DisplaySave(num);
        }
    }
    public void DisplaySave(int num)
    {
        currentPage = 1;
        if (blogSaves[currentSave-1] == null)
        {
            CreateBlogSave(num-1);
            newSaveData = null;
        }
        else
        {
            //sets current save
            newSaveData = blogSaves[num-1];
            newSaveData.SetPage(1);
        }
    }

    public void SaveFile()
    {
        blogSaves[currentSave-1] = null;
        blogSaves.Remove(blogSaves[currentSave-1]);
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