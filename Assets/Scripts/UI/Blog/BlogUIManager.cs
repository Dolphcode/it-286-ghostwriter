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
    private GameObject blogPanel, selectScreen;
    /// <summary>
    /// Indicates which save file the player is on.
    /// </summary>
    private int currentSave;
    /// <summary>
    /// Indicates which page the player is on.
    /// </summary>
    private int currentPage;
    private void Awake()
    {
        blogSave1Button.onClick.AddListener(() => LoadSave(1));
        blogSave2Button.onClick.AddListener(() => LoadSave(2));
        blogSave3Button.onClick.AddListener(() => LoadSave(3));
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        saveButton.onClick.AddListener(CloseBlogPanel);
    }
    void Start()
    {
        int currentPage = 0;
        if (blogSaves != null)
        {
            currentPage = blogSaves[currentSave-1].GetPage().GetPageNum();
        }
    }
    void Update()
    {
        if (blogSaves != null)
        {
            currentPage = 1;
        }
        newSaveData.SetPage(currentPage);
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
    public void CloseBlogPanel()
    {
        blogPanel.SetActive(false);
    }
    public void OpenBlogPanel()
    {
        blogPanel.SetActive(true);
        Debug.Log("BlogPanel on");

    }
    public void CloseSelectScreen()
    {
        selectScreen.SetActive(false);
        Debug.Log("Select off");
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
        blogSaves[i] = newSaveData;
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
        GetScore(blogSaves[x]);
        blogSaves.Remove(blogSaves[currentSave]);
    }
    public void GetScore(int i)
    {
    }
    public void GetScore(BlogSaveData blogSave)
    {
    }
    public void LoadSave(int num)
    {
        currentSave = num-1;
        currentPage = 1;
        if (blogSaves[currentSave] == null)
        {
            CreateBlogSave(currentSave);
            newSaveData = null;
            DisplaySave(num);
        }
        else
        {
            //sets current save
            newSaveData=blogSaves[currentSave];
            DisplaySave(num);
        }
    }
    public void DisplaySave(int num)
    {
        currentSave = num - 1;
        currentPage = 1;
        if (blogSaves[currentSave] == null)
        {
            CreateBlogSave(currentSave);
            newSaveData = null;
        }
        else
        {
            //sets current save
            newSaveData = blogSaves[currentSave];
            newSaveData.SetPage(1);
        }
    }

    public void SaveFile(int num)
    {
        blogSaves[currentSave] = null;
        blogSaves.Remove(blogSaves[currentSave]);
        newSaveData = null;
    }
    public void SetButtonActive()
    {
        if (currentPage == 0) prevButton.interactable = false; nextButton.interactable = true;
        if (currentPage == 1) prevButton.interactable = true; nextButton.interactable = true;
        if (currentPage == 2) prevButton.interactable = true; nextButton.interactable = false;
    }
    public void SetBlogSave(int i)
    {
        currentSave = i;
    }
}