using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Allows the player to control the camera using their mouse
/// </summary>
public class CamControl : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform player;
    public Transform playerModel;
    float xRotation;
    float yRotation;

    public TMP_Text lookingName;
    public Image crosshair;

    public RaycastHit lookingAt;
    public Camera camera;

    [Header("References")]
    public InventoryHolder inventory;
    public Canvas computerInfoCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ///<summary>
        /// Makes the cursor invisible & locks it in the window
        /// </summary>

       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.color = Color.white;
        camera = GetComponent<Camera>();
        
    }

    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f, 90f);
        playerModel.eulerAngles = new Vector3(playerModel.eulerAngles.x, yRotation, playerModel.eulerAngles.z);

        player.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        Shader.SetGlobalVector("_World_Space_Light_Position", player.transform.position);
        Shader.SetGlobalVector("_Spotlight_Direction", player.transform.rotation * Vector3.forward);

        // Always be checking raycast
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        

        if (Physics.Raycast(ray, out lookingAt, 10f, ~LayerMask.GetMask("BoundingBox")))
        {
            // Check for pickups
            if (lookingAt.collider != null)
            {
                if (lookingAt.collider.GetComponent<ItemBehavior>() != null && Input.GetKeyDown(KeyCode.E))
                {
                    inventory.InventorySystem.PickUpItem(lookingAt.collider.GetComponent<ItemBehavior>(), inventory.itemContainer);
                    Debug.Log("Picking up item");
                }

                if (lookingAt.collider.GetComponent<ItemBehavior>() != null)
                {
                    lookingName.text = lookingAt.collider.gameObject.GetComponent<ItemBehavior>().data.Name;
                }
                else
                {
                    lookingName.text = " ";
                }
                if (lookingAt.transform.gameObject.CompareTag("Interactable"))
                {
                    //Set to red for now, add sprite later.
                    crosshair.color = Color.red;
                }
                else
                {
                    crosshair.color = Color.white;
                }
            }

            // Check for player interactable
            if (lookingAt.collider.gameObject.GetComponent<PlayerInteractable>() != null)
            {
                // Check if we're clicking a button
                if (Input.GetKeyDown(KeyCode.Mouse0)) lookingAt.collider.gameObject.GetComponent<PlayerInteractable>().interact();
                
                // Show zoom in screen
                
                if (Input.GetKey(KeyCode.Mouse1) && lookingAt.collider.tag == "Computer")
                {
                    computerInfoCanvas.gameObject.SetActive(true);
                }
                else
                {
                    computerInfoCanvas.gameObject.SetActive(false);
                }
            } else if (lookingAt.collider.tag == "Computer")
            {
                // Show zoom in screen
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    computerInfoCanvas.gameObject.SetActive(true);
                }
                else
                {
                    computerInfoCanvas.gameObject.SetActive(false);
                }
            } else
            {
                computerInfoCanvas.gameObject.SetActive(false);
            }
        } else
        {
            computerInfoCanvas.gameObject.SetActive(false);
        }

        // Left Mouse Click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (inventory != null && inventory.GetHeldItem() != null)
            {
                
                inventory.GetHeldItem().GetComponent<ItemBehavior>().Interact();
            }
        }

    }
}
