using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlogUIManager : MonoBehaviour
{
    /// <summary>
    /// Max 3 blog saves, 3 pages each.
    /// </summary>
    [SerializeField]
    private BlogSaveData[] blogSaves;
    [SerializeField]
    private Button blogSave1Button, blogSave2Button, blogSave3Button;
    [SerializeField]
    private Button prevButton, nextButton, saveButton;
    [SerializeField]
    private TextMeshProUGUI blogTextDisplay; 
    [SerializeField]
    private GameObject blogPanel, selectScreen;

    /// <summary>
    /// Indicates which save file the player is on.
    /// </summary>
    private int currentSave = 0;
    /// <summary>
    /// Indicates which page the player is on of the save file.
    /// </summary>
    private int currentPage = 0;

    void Start()
    {
        blogSave1Button.onClick.AddListener(() => OpenBlog(0));
        blogSave2Button.onClick.AddListener(() => OpenBlog(1));
        blogSave3Button.onClick.AddListener(() => OpenBlog(2));
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        saveButton.onClick.AddListener(CloseBlogPanel);
        OpenBlog(0);
    }

    public void OpenBlog(int saveIndex)
    {
        currentSave = saveIndex;
        currentPage = 0;
        UpdatePages();
    }

    public void UpdatePages()
    {
        if (blogSaves[currentSave].getPages().Length > currentPage)
        {
            blogTextDisplay.text = blogSaves[currentSave].getPages()[currentPage].GetText();
        }
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < blogSaves[currentSave].getPages().Length - 1;
    }
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }
    public void NextPage()
    {
        if (currentPage < 2)
        {
            currentPage++;
            UpdatePages();
        }
    }
    public void CloseBlogPanel()
    {
        blogPanel.SetActive(false);
    }
    public void OpenBlogPanel()
    {
        blogPanel.SetActive(true);
        Debug.Log("BlogPanel activated!");

    }
    public void CloseSelectScreen()
    {
        selectScreen.SetActive(false);
    }
    public void OpenSelectScreen()
    {
        selectScreen.SetActive(true);
    }
}
