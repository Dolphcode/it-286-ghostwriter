using UnityEngine;

public class BlogPage : MonoBehaviour
{
    [SerializeField]
    private BlogSaveData blog;
    [SerializeField]
    private CaptureData[] pictures;
    [SerializeField]
    private string textHeader;
    [SerializeField]
    [TextArea(1,5)] 
    private string text;
    [SerializeField]
    [TextArea(0,1)] 
    private string[] captions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //blog
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
    public void SetImage(int i, CaptureData chosenPic)
    {
        int imageNum = Mathf.Clamp(i, 1, 3);
        pictures[imageNum] = chosenPic;
    }
    public string GetText()
    {
        return text;
    }
}
