using UnityEngine;

/// <summary>
/// The base GhostInteractable interface which must be implemented by
/// all objects that the ghost can interact with in the level.
/// </summary>
public interface GhostInteractable
{
    /// <summary>
    /// This property is used to keep track of whether the interactable
    /// object can still be interacted with or not. Can only be read
    /// and will be modified by the specific implementation of 
    /// GhostInteraction.
    /// </summary>
    bool interactable { get; }

    /// <summary>
    /// Call this function to trigger the ghost's interaction with this
    /// object.
    /// </summary>
    void interact();
}
