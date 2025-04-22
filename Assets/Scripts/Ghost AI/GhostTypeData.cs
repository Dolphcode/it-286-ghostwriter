using UnityEngine;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;

[CreateAssetMenu(fileName = "GhostTypeData", menuName = "Scriptable Objects/GhostTypeData")]
public class GhostTypeData : ScriptableObject
{
    public string typeName;
    public float moveSpeed;
    public int aggressionMultiplier;
    public int maxEMF;
    /// <summary>
    /// Ghost possible models list
    ///</summary>
    [SerializeField]
    private List<GameObject> ghostPrefabList;
    /// <summary>
    ///Reference to the ghost model.
    ///</summary>
    public GameObject ghostModel;
    /// <summary>
    ///Reference to the Evil Sebastian ghost model.
    ///</summary>
    [SerializeField]
    private GameObject sebModel;
    /// <summary>
    ///Reference to the female ghost model.
    ///</summary>
    [SerializeField]
    private GameObject femModel;
    /// <summary>
    /// Reference to the male ghost model.
    ///</summary>
    [SerializeField]
    private GameObject mascModel;
    /// <summary>
    /// Interactables list
    ///</summary>
    public List<GhostInteractable> ghostInteractables;
    public GhostTypeData(string type, float speed, int aggro, int emf, List<GameObject> modelList, List<GhostInteractable> interactablesList)
    {
        typeName = type;
        moveSpeed = speed;
        aggressionMultiplier = aggro;
        maxEMF = emf;
        ghostPrefabList = modelList;
        ghostInteractables = interactablesList;
    }
    public void AssignGhostModel(GameObject model)
    {
        foreach (GameObject m in ghostPrefabList)
        {
            if (m == model)
            {
                ghostModel = model;
            }
        }
        if (ghostModel == null)
        {
            Debug.Log("No ghost model assigned.");
        }
    }
    public void IsGhostConstantSpeed(bool isConstant)
    {
    }
}