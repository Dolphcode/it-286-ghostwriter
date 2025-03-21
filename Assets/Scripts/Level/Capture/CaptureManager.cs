using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    [Header("Capturable Objects")]
    [SerializeField]
    private List<GhostInteractable> interactables;
    [SerializeField]
    private List<Ghost> ghosts;

    [SerializeField] 
    private List<CaptureData> captures;

    public void AppendInteractable(GhostInteractable interactable)
    {
        interactables.Append(interactable);
    }

    /// <summary>
    /// Wrapper function to add a ghost to the list of ghosts in this capture manager
    /// </summary>
    /// <param name="ghost"></param>
    public void AppendGhost(Ghost ghost)
    {
        ghosts.Append(ghost);
    }

    /// <summary>
    /// Generate a capture data 
    /// </summary>
    /// <param name="image">The image texture captured by the camera</param>
    /// <param name="camera">The camera reference to evaluate what was contained in the picture</param>
    /// <returns>A reference to the capture data if needed</returns>
    public CaptureData CaptureImage(Texture2D image, Camera camera)
    {
        CaptureData data = new CaptureData();

        // Assign the capture image and timestamp
        data.capture = image;
        data.timestamp = System.DateTime.Now;

        // Compute score and append flags depending on contents of image

        return data;
    }
}
