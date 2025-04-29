using TMPro;
using UnityEngine;

public class BlogPanel : MonoBehaviour
{
    [SerializeField]
    private CaptureData displayImage;
    [SerializeField]
    private TextMeshProUGUI displayHeader;
    [SerializeField]
    private TextMeshProUGUI displayText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDisplayImage(CaptureData displayImage1)
    {
        displayImage = displayImage1;
    }
    public void SetDisplayText(string displayText1) 
    {
        displayText.text = displayText1;
    }
    public void SetDisplayHeader(string header)
    {
        displayHeader.text = header;
    }
}
