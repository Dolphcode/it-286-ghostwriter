using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageScript : MonoBehaviour
{
    [SerializeField] private CaptureManager captureManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Image picture;
    private List<Sprite> spriteEquivs = new List<Sprite>();
    private int imageNumber = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (FindAnyObjectByType<LevelManager>()!= null)
            levelManager = FindAnyObjectByType<LevelManager>();
        if (FindAnyObjectByType<LevelManager>() != null&&levelManager.GetCaptureManager()!=null)
        { 
            captureManager = levelManager.GetCaptureManager();
            while (spriteEquivs.Count < captureManager.CaptureCount)
            {
                Sprite spriteConv = Sprite.Create(captureManager.GetImage(spriteEquivs.Count), new Rect(0, 0, 320, 240), new Vector2(0.5f, 0.5f));
                spriteEquivs.Add(spriteConv);
            }
            if (spriteEquivs.Count == 0)
            {
                picture.gameObject.SetActive(false);
            }
            else
            {
                picture.gameObject.SetActive(true);
                picture.sprite = spriteEquivs[imageNumber];
            }
        }
    }
    public void NextImage()
    {
        if (spriteEquivs.Count == 0) return;
        imageNumber++;
        if (imageNumber >= spriteEquivs.Count) imageNumber = 0;
    }
    public void PrevImage()
    {
        if (spriteEquivs.Count == 0) return;
        imageNumber--;
        if (imageNumber < 0) imageNumber = spriteEquivs.Count - 1;
    }
    public int GetScore()
    {
        return captureManager.GetCaptureData(spriteEquivs.Count).score;
    }
}
