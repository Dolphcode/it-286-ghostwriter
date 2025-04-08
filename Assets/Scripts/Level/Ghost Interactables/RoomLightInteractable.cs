using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// The base GhostInteractable interface which must be implemented by
/// all objects that the ghost can interact with in the level.
/// </summary>
public class RoomLightInteractable : GhostInteractable
{
    [SerializeField]
    private List<MeshRenderer> litMeshes;

    [SerializeField]
    private bool currentState;

    /// <summary>
    /// This variable exists so that we don't have to modify the mesh variable
    /// every frame, instead we check if currentState and storedState differ.
    /// </summary>
    private bool storedState = false;

    /// <summary>
    /// Append a lit mesh to this room light interactable's lit mesh list
    /// </summary>
    /// <param name="mesh">The mesh renderer to be appended</param>
    public void AddLitMesh(MeshRenderer mesh)
    {
        litMeshes.Add(mesh);
    }

    private void Awake()
    {
        m_TriggerCapture = new UnityEvent<Capturable>();
        storedState = currentState;
        foreach (var l in litMeshes)
        {
            l.material = Instantiate(l.material); // Make material unique to object
            l.material.SetFloat("_Light_On_Interior", (storedState ? 1f : 0f));
        }
    }

    private void Update()
    {
        if (storedState !=  currentState)
        {
            storedState = currentState;
            foreach (var l in litMeshes)
            {
                l.material.SetFloat("_Light_On_Interior", (storedState ? 1f : 0f));
            }
        }
    }

    /// <summary>
    /// Call this function to switch the light's state
    /// object.
    /// </summary>
    public override void interact()
    {
        currentState = !currentState;
        m_TriggerCapture.Invoke(this); // Only remote cameras can capture this
    }
}
