using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unnamed Ghost Type", menuName = "GhostTypeData")]
public class GhostTypeData : ScriptableObject
{
    [Header("Ghost Metadata")]
    public string typeName; // the type of the ghost

    [Header("Ghost Evidence Config")]
    [Range(1, 5)] public int maxEMF; // maximum EMF that the ghost can hit
    public List<GhostInteractableType> interactableTypes; // The list of things a ghost can interact with
    public float maxSpeed; // The maximum speed of the ghost
    public bool adjustableSpeed; // Whether the ghost will adjust its speed or not
}
