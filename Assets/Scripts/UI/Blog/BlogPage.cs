using TMPro;
using UnityEngine;
public class BlogPage : MonoBehaviour
{
    [SerializeField]
    private BlogSaveData blog;
    [SerializeField]
    private int pageNumber;
    [SerializeField]
    private CaptureData image;
    [SerializeField]
    private TextMeshProUGUI textHeader;
    [SerializeField]
    [TextArea(1,5)] 
    private TextMeshProUGUI bodyText;
    [SerializeField]
    [TextArea(0,1)]
    private TextMeshProUGUI tags;
    [SerializeField]
    [TextArea(0,1)] 
    private System.Collections.Generic.Dictionary<CaptureData, TextMeshProUGUI> imageCaptions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pageNumber == 1)
        {
            bodyText.text = blog.GetLevelLoadscreenData().loadingDescription;
            textHeader.text = "Ghost Writer: " + blog.GetLevelLoadscreenData().levelName;
        }
        else if (pageNumber == 2)
        {
            bodyText.text = blog.GetLevelLoadscreenData().loadingDescription;
        }
        else if (pageNumber == 3)
        {
           foreach (string tag in blog.GetTags())
            {
                tags.text += "#" + tag + ", ";
            }
           // Removes last comma
            tags.text.Substring(tags.text.Length - 1);
        }    
    }
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Given image number to be set and picture
    /// </summary>
    /// <param name="i"></param>
    /// <param name="pic"></param>
    public void SetImage(CaptureData givenImage)
    {
        image = givenImage;
    }
    public int GetPageNum()
    {
        return pageNumber;
    }
}
