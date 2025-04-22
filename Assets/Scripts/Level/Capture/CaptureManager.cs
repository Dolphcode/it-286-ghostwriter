using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private List<CapturableObject> capturableObjects; // Scriptable object version

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private Canvas debugCanvas;
    [SerializeField]
    private TextMeshProUGUI remoteTimestampLabel;
    [SerializeField]
    private TextMeshProUGUI remoteScoreLabel;
    [SerializeField]
    private TextMeshProUGUI handheldTimestampLabel;
    [SerializeField]
    private TextMeshProUGUI handheldScoreLabel;
    [SerializeField]
    private Image remoteIcon;
    [SerializeField]
    private Image handheldIcon;


    [SerializeField] 
    private List<CaptureData> captures;
    
    public int CaptureCount { private set { } get { return captures.Count; } }
    public Texture2D GetImage(int index) { return captures[index].capture; }
    public CaptureData GetCaptureData(int index) { return captures[index]; }

    public void Awake()
    {
        if (debug)
        {
            debugCanvas.enabled = true;
        } else
        {
            debugCanvas.enabled = false;
        }
        behaviors = new List<ItemBehavior>();
        interactables = new List<GhostInteractable>();
        ghosts = new List<Ghost>();
        capturables = new List<Capturable>();
    }

    public void Update()
    {
        if (debug)
        {
            bool handheldFound = false, remoteFound = false;
            for (int i = captures.Count() - 1; i >= 0; --i)
            {
                if (captures[i].remoteCapture && !remoteFound)
                {
                    remoteFound = true;
                    remoteIcon.sprite = Sprite.Create(captures[i].capture, new Rect(0, 0, 320, 240), new Vector2(0, 0));
                    remoteScoreLabel.text = captures[i].score.ToString();
                    remoteTimestampLabel.text = captures[i].timestamp.ToShortTimeString();
                } else if (!handheldFound)
                {
                    handheldFound = true;
                    handheldIcon.sprite = Sprite.Create(captures[i].capture, new Rect(0, 0, 320, 240), new Vector2(0, 0));
                    handheldScoreLabel.text = captures[i].score.ToString();
                    handheldTimestampLabel.text = captures[i].timestamp.ToShortTimeString();
                }

                if (remoteFound && handheldFound) break;
            }

            if (!handheldFound)
            {
                handheldIcon.sprite = null;
                handheldScoreLabel.text = "";
                handheldTimestampLabel.text = "";
            }

            if (!remoteFound)
            {
                remoteIcon.sprite = null;
                remoteScoreLabel.text = "";
                remoteTimestampLabel.text = "";
            }
        }
    }

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

    public void AppendCapturable(Capturable capturable)
    {
        capturables.Add(capturable); 
    }

    public void AppendCapturableObject(CapturableObject capturableObject)
    {
        capturableObjects.Add(capturableObject);
    }

    public void RegisterEventListener(UnityAction<Capturable> callback)
    {
        Debug.Log("registering events " + capturables.Count().ToString());
        foreach (Capturable c in capturables)
        {
            Debug.Log(c.name);
            
            if (c.HasTriggerCaptureEvent())
            {
                c.AddTriggerListener(callback);
            }
        }
    }

    public void RegisterEventListener(UnityAction<CapturableObject> callback)
    {
        Debug.Log("registering events " + capturableObjects.Count().ToString());
        foreach (CapturableObject c in capturableObjects)
        {
            Debug.Log(c.name);

            if (c.HasTriggerCaptureEvent())
            {
                c.AddTriggerListener(callback);
            }
        }
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


        foreach (Capturable capturable in capturables)
        {
            Transform t = capturable.GetCheckObject().transform;
            Vector3 screenpos = camera.WorldToViewportPoint(t.position);
            Debug.Log(screenpos.ToString());
            // First test if it is on screen
            if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 && screenpos.z >= 0)
            {
                Debug.Log(t.name + " is in the frustum and is object " + t.gameObject.name);
                // Now fire off a raycast to check for anything blocking
                // TODO: Find a more accurate way to do this?
                RaycastHit hit;
                if (Physics.Raycast(camera.transform.position,
                    (t.position - camera.transform.position).normalized,
                    out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == capturable.GetCheckObject())
                {
                    Debug.Log(hit.collider.gameObject.name + " was hit");
                    data.score += capturable.GetCaptureScore(1f, data);
                }
            }
        }

        foreach (CapturableObject capturable in capturableObjects)
        {
            Transform t = capturable.GetCheckObject().transform;
            Vector3 screenpos = camera.WorldToViewportPoint(t.position);
            Debug.Log(screenpos.ToString());
            // First test if it is on screen
            if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 && screenpos.z >= 0)
            {
                Debug.Log(t.name + " is in the frustum and is object " + t.gameObject.name);
                // Now fire off a raycast to check for anything blocking
                // TODO: Find a more accurate way to do this?
                RaycastHit hit;
                if (Physics.Raycast(camera.transform.position,
                    (t.position - camera.transform.position).normalized,
                    out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == capturable.GetCheckObject())
                {
                    Debug.Log(hit.collider.gameObject.name + " was hit");
                    data.score += capturable.GetCaptureScore(1f, data);
                }
            }
        }


        captures.Add(data);
        return data;
    }
}
