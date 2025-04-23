using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlogSaveData", menuName = "ScriptableObjects/BlogSaveData")]
public class BlogSaveData : ScriptableObject
{
    /// <summary>
    /// List of photos taken by the player from both camera and remote camera
    /// </summary>
    [SerializeField]
    private List<CaptureData> photosList;
    /// <summary>
    /// List of each ghost in level's data
    /// </summary>
    [SerializeField]
    private List<GhostTypeData> ghostDataList;
    /// <summary>
    /// List of correct answers for captions
    /// </summary>
    [SerializeField]
    private List<string> answers;
    /// <summary>
    /// List of given answers for captions
    /// </summary>
    [SerializeField]
    private List<object> prompts;
    /// <summary>
    /// List of pages.
    /// </summary>
    [SerializeField]
    private BlogPage[] pages;
    /// <summary>
    /// List of given answers for captions
    /// </summary>
    [SerializeField]
    private int currentScore;
    /// <summary>
    /// List of tags for blog
    /// </summary>
    [SerializeField]
    private string[] tags;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; ghostDataList.Count >= i; i++)
        {
            answers.Add(ghostDataList[i].typeName);
            answers.Add(ghostDataList[i].maxEMF.ToString());
            answers.Add(ghostDataList[i].maxSpeed.ToString());
            answers.Add(ghostDataList[i].adjustableSpeed.ToString());
            for (int c = 0; ghostDataList[i].interactableTypes.Count >= c; c++)
            {
                answers.Add(ghostDataList[i].interactableTypes[c].ToString());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public BlogPage[] getPages()
    {
        return pages;
    }
}
