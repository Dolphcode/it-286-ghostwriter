using UnityEngine;

public class Blog : MonoBehaviour
{
    [SerializeField]
    private blogEntry blog;
    [SerializeField]
    private CaptureData image1;
    [SerializeField]
    private CaptureData image2;
    [SerializeField]
    private CaptureData image3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blog = ;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetImage(int i, CaptureData pic)
    {
        int imageNum = Mathf.Clamp(i, 1, 3);
        if (imageNum == 1) image1 = pic;
        else if (imageNum == 2) image2 = pic;
        else image3 = pic;
    }
    public void SetImage(int i, CaptureData pic)
    {
        int imageNum = Mathf.Clamp(i, 1, 3);
        if (imageNum == 1) image1 = pic;
        else if (imageNum == 2) image2 = pic;
        else image3 = pic;
    }
}
