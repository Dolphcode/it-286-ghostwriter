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
    private int pageNum;
    [SerializeField]
    private CaptureData imageData;
    [SerializeField]
    private Texture2D displayImage;
    [SerializeField]
    private TextMeshProUGUI displayHeader;
    [SerializeField]
    private GhostTypeData ghostData;
    [SerializeField] 
    private TMP_Dropdown dropdown1;
    [SerializeField]
    private TMP_Dropdown dropdown2;
    [SerializeField]
    private TextMeshProUGUI bodyText1;
    [SerializeField]
    private TextMeshProUGUI bodyText2;
    /// <summary>
    /// Answers but in the corresponding string answer form
    /// </summary>
    [SerializeField] 
    private List<string> correctAnswersString = new List<string>();
    /// <summary>
    /// Answers but in their original form (bool,string,int,etc)
    /// </summary>
    [SerializeField]
    private List<object> correctAnswers = new List<object>();
    /// <summary>
    /// Correct Sentences
    /// </summary>
    [SerializeField]
    private List<Sentence> correctSentences = new List<Sentence>();
    /// <summary>
    /// Given sentences from user
    /// </summary>
    [SerializeField]
    private List<Sentence> givenSentences = new List<Sentence>();
    /// <summary>
    /// Score report for sentences in list form
    /// </summary>
    [SerializeField]
    private List<bool> checkedSentences = new List<bool>();
    /// <summary>
    /// Number of true (aka correct answer) in checkedSentences
    /// </summary>
    [SerializeField]
    private int sentenceScore;

    private string interactableAns1;
    private string interactableAns2;
    private bool sent1 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCorrectAnswers();
        pageNum = 1;
        dropdown1.onValueChanged.AddListener(OnDropdownChanged);
        dropdown2.onValueChanged.AddListener(OnDropdownChanged);
        
    }
    public void OnDropdownChanged(int answerIndex)
    {
        SetPlayerAnswers();
    }
    public void SetPlayerAnswers()
    {
        DropdownReset(1);
        for (int i1 = 0; i1 < givenSentences.Count; i1++)
        {
            givenSentences[i1] = null;
        }
        givenSentences.Add(new Sentence("When I went back to one of the rooms I explored previously, I saw it. ", dropdown1.options[dropdown1.value].text));
        givenSentences.Add(new Sentence("Then later on, something unusual happened again. ", dropdown2.options[dropdown2.value].text));
    }
    public void SetCorrectAnswers()
    {
        //if (blog != null)
            //correctAnswers = blog.GetAnswers();
        //else
        //{
            /*if (ghostData != null)
            {
                correctAnswers.Add(ghostData.typeName);
                correctAnswers.Add(ghostData.maxEMF);
                correctAnswers.Add(ghostData.adjustableSpeed);
                correctAnswers.Add("door");
                correctAnswers.Add("move");
            }
            else*/
            {
                correctAnswers.Add("Biological");
                correctAnswers.Add(3);
                correctAnswers.Add(false);
                correctAnswers.Add("door");
                correctAnswers.Add("move");
            }
                
            //for (int c = 0; ghostData.interactableTypes.Count >= c; c++)
            {
                //correctAnswers.Add(ghostData.interactableTypes[c].ToString());
                //should return game.GhostInteractables.DoorInteractable or DoorInteractable?
            }
        //}
        // Clears sentences
        if (correctSentences.Count>0)
        {
            for (int i1 = 0; i1 < correctSentences.Count; i1++)
            {
                correctSentences[i1] = null;
            }
        }
        for (int i1 = 0; i1 < correctAnswersString.Count; i1++)
        {
            correctAnswersString[i1] = null;
        }
        for (int i = 0; i < correctAnswers.Count; i++)
        {
            if (correctAnswers[i] is not string)
            {
                // Speed
                if (correctAnswers[i] is bool)
                {
                    if ((bool)(correctAnswers[i]))
                    {
                        correctAnswersString.Add("erratically, as if it couldn’t decide how fast to go.");
                    }
                    else
                        correctAnswersString.Add("at a constant speed.");
                }
                // EMF
                if (correctAnswers[i] is int)
                {
                    if ((int)(correctAnswers[i]) > 4)
                        correctAnswersString.Add("very high.");
                    else if ((int)(correctAnswers[i]) == 3)
                        correctAnswersString.Add("relatively normal.");
                    else if ((int)(correctAnswers[i]) < 3)
                        correctAnswersString.Add("somewhat low.");
                    else
                        correctAnswersString.Add("no maxEMF found.");
                }
            }
            else correctAnswersString.Add(correctAnswers[i].ToString().Trim().ToLower());
        }
        //added in order typeName, maxEMF, adjustableSpeed, interactableTypes - sentences randomized
        if (Random.value > 0.5f)
        {
            correctSentences.Add(new Sentence("The ghost’s behaviors were similar to those of a ", correctAnswersString[0] + " type ghost."));
            sent1 = true;
        }
        else
            correctSentences.Add(new Sentence("From the ghost’s behavior, I identified it to be a ", correctAnswersString[0] + " ghost."));
        correctSentences.Add(new Sentence("The ghost’s maximum EMF was ", correctAnswersString[1]));
        correctSentences.Add(new Sentence("The ghost was moving ", correctAnswersString[2]));
        foreach (string s in correctAnswersString)
        {
            if (interactableAns1 == null)
            {
                if (s.Contains("print")) interactableAns1 = "A bright red handprint.";
                if (s.Contains("light")) interactableAns1 = "The lights flickered.";
                if (s.Contains("drawer")) interactableAns1 = "A drawer I hadn’t touched before, mysteriously open.";
                if (s.Contains("door")) interactableAns1 = "The door creaked open before my eyes.";
                if (s.Contains("move")) interactableAns1 = "The chair moved slowly, back and forth.";
            }
            else
            {
                if (s.Contains("print") && !interactableAns1.Contains("print")) interactableAns2 = "A bright red handprint appeared on the window.";
                if (s.Contains("light") && !interactableAns1.Contains("light")) interactableAns2 = "The lights flickered.";
                if (s.Contains("drawer") && !interactableAns1.Contains("drawer")) interactableAns2 = "A drawer I hadn’t touched before, mysteriously opened.";
                if (s.Contains("door") && !interactableAns1.Contains("door")) interactableAns2 = "The door creaked open before my eyes.";
                if (s.Contains("move") && !interactableAns1.Contains("move")) interactableAns2 = "The rocking chair, which I was nowhere near, moved.";
            }
        }
        correctSentences.Add(new Sentence("When I went back to one of the rooms I explored previously, I saw it. ", interactableAns1));
        correctSentences.Add(new Sentence("Then later on, something unusual happened again. ", interactableAns2));
    }
    public List<bool> CheckCorrectAnswers()
    {
        //assumes sentence order is the same each time
        for (int i = 0; correctSentences.Count < i; i++)
        {
            if (correctSentences[i] == givenSentences[i])
                checkedSentences.Add(true);
            else
                checkedSentences.Add(false);
        }
        return checkedSentences;
    }
    public float GetSentencePercent()
    {
        float trues = 0;  float total = 0;
        foreach (bool b in checkedSentences)
        {
            if (b)
            {
                trues++; 
                total++;
            }
            else
                total++;

        }
        return trues / total;
    }
    public int GetSentenceScore()
    {
        int trues = 0;
        foreach (bool b in checkedSentences)
        {
            if (b) trues++;
        }
        return trues;
    }
    public int GetImageScore()
    {
        return imageData.score;
    }
    public void SetDropdownOptions(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();           
        dropdown.AddOptions(options);        
        dropdown.value = 0;                 
        dropdown.RefreshShownValue();       
    }
    // Update is called once per frame
    void Update()
    {
        displayHeader.gameObject.SetActive(true);
        bodyText1.text = blog.GetLevelLoadscreenData().loadingDescription;
        if (displayHeader.text == null)
            displayHeader.text = "Ghost Writer: " + blog.GetLevelLoadscreenData().levelName;
        dropdown1.gameObject.SetActive(true);
        dropdown2.gameObject.SetActive(true);
        //blog.GetPage().text1 = bodyText1.text;
        //blog.GetPage().text2 = bodyText2.text;
        DropdownReset(1);
    }
    public void DropdownReset(int i)
    {
        dropdown1.ClearOptions();
        dropdown2.ClearOptions();
        //where i=pageNum
        if (i == 1)
        {
            List<string> list1 = new List<string>();
            list1.Add("A bright red handprint.");
            list1.Add("The lights flickered.");
            list1.Add("A drawer I hadn’t touched before, mysteriously open.");
            list1.Add("The door creaked open before my eyes.");
            list1.Add("The chair moved slowly, back and forth.");
            dropdown1.AddOptions(list1);
            List<string> list2 = new List<string>();
            list2.Add("A bright red handprint appeared on the window.");
            list2.Add("The lights flickered.");
            list2.Add("A drawer I hadn’t touched before, mysteriously openED.");
            list2.Add("The door creaked open before my eyes.");
            list2.Add("The rocking chair, which I was nowhere near, moved.");
            dropdown2.AddOptions(list2);
        }
    }
    public void SetDisplayImage(Texture2D displayImage1)
    {
        displayImage = displayImage1;
    }
    public void SetDisplayHeader(string header)
    {
        displayHeader.text = header;
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
