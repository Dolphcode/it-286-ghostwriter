using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelComputerManager : MonoBehaviour
{
    [Header("Status References")]
    [SerializeField] private TextMeshProUGUI fearMeter;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] Fear fearHandler;
    [SerializeField] private GameObject confirmButtonObj;
    [SerializeField] private GameObject confirmButtonUI;
    [SerializeField] private GameObject confirmText;

    [Header("Photos References")]
    [SerializeField] private CaptureManager captureManager;
    [SerializeField] private Image picture;

    [Header("Other References")]
    [SerializeField] private LevelManager levelManager;

    private float lifetime = 0f;
    private bool leaving;

    private List<Sprite> spriteEquivs = new List<Sprite>();
    private int imageNumber = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
        fearHandler = FindAnyObjectByType<Fear>();
    }

    // Update is called once per frame
    void Update()
    {
        // The status screen
        if (leaving)
        {
            confirmButtonObj.SetActive(true);
            confirmButtonUI.SetActive(true);
            confirmText.SetActive(true);
        } else
        {
            confirmButtonObj.SetActive(false);
            confirmButtonUI.SetActive(false);
            confirmText.SetActive(false);
        }

        if (levelManager.levelStarted)
        {
            lifetime += Time.deltaTime;
        }
        fearMeter.text = (Mathf.RoundToInt(fearHandler.fearMeter)).ToString() + "%";
        fearMeter.color = new Color((fearHandler.fearMeter / 100f), 1f - (fearHandler.fearMeter / 100f), 0f);
        TimeSpan timeSpan = TimeSpan.FromSeconds(lifetime);
        time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        // The photos screen
        while (spriteEquivs.Count < captureManager.CaptureCount)
        {
            Sprite spriteConv = Sprite.Create(captureManager.GetImage(spriteEquivs.Count), new Rect(0, 0, 320, 240), new Vector2(0, 0));
            spriteEquivs.Add(spriteConv);
        }

        if (spriteEquivs.Count == 0)
        {
            picture.gameObject.SetActive(false);
        } else
        {
            picture.gameObject.SetActive(true);
            picture.sprite = spriteEquivs[imageNumber];
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

    public void CancelLeave()
    {
        leaving = false;
    }

    public void Leave()
    {
        if (leaving)
        {
            levelManager.ExitLevel();
        } else
        {
            leaving = true;
        }
    }
}
