using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class blogEntry : MonoBehaviour
{
    [SerializeField]
    private List<CaptureData> photosList;
    [SerializeField]
    private List<GhostTypeData> ghostDataList;
    [SerializeField]
    private List<object> answers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        answers.Add(ghostDataList[1].typeName);
        answers.Add(ghostDataList[2].maxEMF);
        answers.Add(ghostDataList[3].ghostInteractables);
        answers.Add(ghostDataList[4].ghostModel);
        answers.Add(ghostDataList[5].isConstantSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
