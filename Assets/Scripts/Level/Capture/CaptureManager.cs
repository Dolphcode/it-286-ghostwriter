using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaptureManager : MonoBehaviour
{
    [Header("Capturable Objects")]
    [SerializeField]
    private List<ItemBehavior> behaviors;
    [SerializeField]
    private List<GhostInteractable> interactables;
    [SerializeField]
    private List<Ghost> ghosts;
    [SerializeField]
    private List<Capturable> capturables;

    [SerializeField] 
    private List<CaptureData> captures;

    /// <summary>
    /// Wrapper function to add an item behavior object to the list of behaviors in this capture manager
    /// </summary>
    /// <param name="itemBehavior"></param>
    public void AppendItemBehavior(ItemBehavior itemBehavior)
    {
        behaviors.Add(itemBehavior);
    }

    /// <summary>
    /// Wrapper function to add an interactable to the list of ghost interactables in this capture manager
    /// </summary>
    /// <param name="interactable"></param>
    public void AppendInteractable(GhostInteractable interactable)
    {
        interactables.Add(interactable);
    }

    /// <summary>
    /// Wrapper function to add a ghost to the list of ghosts in this capture manager
    /// </summary>
    /// <param name="ghost"></param>
    public void AppendGhost(Ghost ghost)
    {
        ghosts.Add(ghost);
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
        // First check if the ghost is in view
        foreach (Ghost ghost in ghosts)
        {
            Vector3 screenpos = camera.WorldToViewportPoint(ghost.transform.position);
            Debug.Log(screenpos.ToString());
            // First test if it is on screen
            if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 &&  screenpos.z >= 0)
            {
                Debug.Log("ghost in the frustum!");
                // Now fire off a raycast to check for anything blocking
                // TODO: Find a more accurate way to do this?
                RaycastHit hit;
                if (Physics.Raycast(camera.transform.position, 
                    (ghost.transform.position - camera.transform.position).normalized, 
                    out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == ghost.gameObject)
                {
                    Debug.Log("ghost in view!");
                    if (ghost.IsGhostHunting())
                    {
                        data.score += 5;
                    }
                    else
                    {
                        data.score += 2; //????
                    }
                }
            }
        }


        captures.Add(data);
        return data;
    }
}
