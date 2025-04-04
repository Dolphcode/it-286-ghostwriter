using UnityEngine;

using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// The base PlayerInteractable interface which must be implemented by
/// all objects that the ghost can interact with in the level.
/// </summary>
public abstract class PlayerInteractable : MonoBehaviour
{

    private MeshRenderer m_Renderer;
    private void Awake()

    {
        
        interactable = true;
        m_Renderer = GetComponent<MeshRenderer>();
    }

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

}

