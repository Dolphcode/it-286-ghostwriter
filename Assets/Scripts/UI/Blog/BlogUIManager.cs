using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    // <summary>
    // Dictionary that stores blog save and current page number.
    // </summary>
    //[SerializeField]
    //private System.Collections.Generic.Dictionary<BlogSaveData, int> blogDictionary = new Dictionary<BlogSaveData, int>();
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
    private int currentSave = 0;
    /// <summary>
    /// Indicates which page the player is on.
    /// </summary>
    private int currentPage = 0;
    private void Awake()
    {
        blogSave1Button.onClick.AddListener(() => OpenBlog(0));
        blogSave2Button.onClick.AddListener(() => OpenBlog(1));
        blogSave3Button.onClick.AddListener(() => OpenBlog(2));
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
        OpenBlog(0);
    }
    public void OpenBlog(int saveIndex)
    {
        //blogDictionary.Add(blogSaves[saveIndex], 0);
        currentSave = saveIndex;
        currentPage = 0;
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
    public void SetCurrentBlog(int i)
    {
        newSaveData = blogSaves[i - 1];
    }
    public List<BlogSaveData> GetBlogSaves()
    {
        return blogSaves;
    }
    public void RemoveBlogSave(BlogSaveData remove)
    {
        blogSaves.Remove(remove);
    }
    public void Publish()
    {
        blogSaves[currentSave] = null;
        blogSaves.Remove(blogSaves[currentSave]);
    }
}