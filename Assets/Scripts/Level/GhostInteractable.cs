using UnityEngine;
using UnityEngine.Events;

public enum GhostInteractableType
{
    Lights, // For any ghost interactables that affect lighting
    Movable, // For any ghost interactables that can be moved
    Fingerprint // For leaving fingerprints
}

/// <summary>
/// The base GhostInteractable interface which must be implemented by
/// all objects that the ghost can interact with in the level.
/// </summary>
public abstract class GhostInteractable : Capturable
{

    private MeshRenderer m_Renderer;
    private void Awake()
    {
        interactable = true;
        m_Renderer = GetComponent<MeshRenderer>();
        m_TriggerCapture = new UnityEvent();
    }

    /// <summary>
    /// The specific category of interactable that this interactable is
    /// </summary>
    [SerializeField]
    GhostInteractableType interactableType;
    public GhostInteractableType GetInteractableType() { return interactableType; }

    /// <summary>
    /// This property is used to keep track of whether the interactable
    /// object can still be interacted with or not. Can only be read
    /// and will be modified by the specific implementation of 
    /// GhostInteraction.
    /// </summary>
    public bool interactable;

    /// <summary>
    /// Call this function to trigger the ghost's interaction with this
    /// object.
    /// </summary>
    public abstract void interact();

    public override GameObject GetCheckObject()
    {
        return gameObject;
    }

    public override int GetCaptureScore(float rayProp, CaptureData data)
    {
        if (!interactable)
        {
            data.evidenceCount++;
            return 5;
        } else
        {
            return 0;
        }
    }

}
