using UnityEngine;

/// <summary>
/// Represents the interactable object for the ghost to leave fingerprints
/// </summary>
public class FingerprintInteractable : GhostInteractable
{
    [SerializeField]
    private MeshRenderer fingerprintMesh;

    public override void interact()
    {
        interactable = false;
        fingerprintMesh.enabled = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fingerprintMesh.enabled = false;   
    }
}
