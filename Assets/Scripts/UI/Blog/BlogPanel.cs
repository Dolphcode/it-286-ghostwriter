using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class BlogPanel : MonoBehaviour
{
    [SerializeField]
    private BlogSaveData blog;
    [SerializeField]
    private BlogUIManager blogUI;
    [SerializeField]
    private TextMeshProUGUI displayHeader;
    [SerializeField]
    private TextMeshProUGUI bodyText1;
    [SerializeField]
    private TextMeshProUGUI bodyText2;
    [SerializeField]
    private TextMeshProUGUI bodyText3;
    [SerializeField]
    private TextMeshProUGUI ans1a, ans1b, ans1c;
    [SerializeField]
    private TextMeshProUGUI ans2a, ans2b, ans2c;
    private int ans1, ans2, givenAns1, givenAns2;
    private bool quesOrder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (blog != null)
            SetQandA();
        else
            Debug.Log("NO BLOG");
       quesOrder = Random.value > 0.5f;
    }
    public void SetQandA()
    {
        if (blog.GetGhostData() != null)
        {
            if (quesOrder)
            {
                bodyText2.text = "The highest EMF of the ghost was...";
                ans1a.text = "1-2";
                ans1b.text = "3-4";
                ans1c.text = "5+";
                if (blog.GetGhostData().maxEMF<3)
                {
                    ans1 = 1;
                }
                else if (blog.GetGhostData().maxEMF < 5)
                {
                    ans1 = 2;
                }
                else
                {
                    ans1 = 3;
                }
                bodyText3.text = "The ghost had an unusual aura. I think its type was...";
                ans2a.text = "Biological";
                ans2b.text = "Metaphysical";
                ans2c.text = "Psychological";
                ans2a.fontSize = 3;
                ans2b.fontSize = 3;
                ans2c.fontSize = 3;
                if (blog.GetGhostData().typeName == "Biological")
                {
                    ans2 = 1;
                }
                else if (blog.GetGhostData().typeName == "Metaphysical")
                {
                    ans2 = 2;
                }
                else if (blog.GetGhostData().typeName == "Psychological")
                {
                    ans2 = 3;
                }
            }
            else
            {
                bodyText2.text = "The ghost had an unusual aura. I think its type was...";
                ans1a.text = "Biological";
                ans1b.text = "Metaphysical";
                ans1c.text = "Psychological";
                ans1a.fontSize = 3;
                ans1b.fontSize = 3;
                ans1c.fontSize = 3;
                if (blog.GetGhostData().typeName == "Biological")
                {
                    ans1 = 1;
                }
                else if (blog.GetGhostData().typeName == "Metaphysical")
                {
                    ans1 = 2;
                }
                else if (blog.GetGhostData().typeName == "Psychological")
                {
                    ans1 = 3;
                }
                bodyText3.text = "The highest EMF of the ghost was...";
                ans2a.text = "1-2";
                ans2b.text = "3-4";
                ans2c.text = "5+";
                if (blog.GetGhostData().maxEMF < 3)
                {
                    ans2 = 1;
                }
                else if (blog.GetGhostData().maxEMF < 5)
                {
                    ans2 = 2;
                }
                else
                {
                    ans2 = 3;
                }
            }
        }
    }
    public void ConfirmAnswer1(int i)
    {
        givenAns1 = i;
    }
    public void ConfirmAnswer2(int i)
    {
        givenAns2 = i;
    }
    public int GetQuestionScore()
    {
        int score = 0;
        if (ans1 == givenAns1)
            score += 1;
        if (ans2==givenAns2)
            score += 1;
        return score;
    }
    // Update is called once per frame
    void Update()
    {
        if (blog != null) SetQandA();
        displayHeader.gameObject.SetActive(true);
        bodyText1.text = blog.GetLevelLoadscreenData().loadingDescription;
        if (displayHeader.text == null)
            displayHeader.text = "Ghost Writer: " + blog.GetLevelLoadscreenData().levelName;
    }
    public string GetDisplayHeader()
    {
        return displayHeader.text;
    }
    public void LoadSaveData(BlogSaveData newSaveData)
    {
        blog = newSaveData;
    }
}
