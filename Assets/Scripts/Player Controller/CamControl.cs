using UnityEngine;
/// <summary>
/// Allows the player to control the camera using their mouse
/// </summary>
public class CamControl : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform player;
    float xRotation;
    float yRotation;

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

        camera = GetComponent<Camera>();
        
    }

    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f, 90f);

        player.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        Shader.SetGlobalVector("_World_Space_Light_Position", player.transform.position);
        Shader.SetGlobalVector("_Spotlight_Direction", player.transform.rotation * Vector3.forward);

        // Always be checking raycast
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out lookingAt))
        {
            if (lookingAt.collider.gameObject.GetComponent<PlayerInteractable>() != null)
            {
                // Check if we're clicking a button
                if (Input.GetKeyDown(KeyCode.Mouse0)) lookingAt.collider.gameObject.GetComponent<PlayerInteractable>().interact();
                
                // Show zoom in screen
                if (Input.GetKey(KeyCode.Mouse1))
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
            

            if (inventory.GetHeldItem() != null)
            {
                inventory.GetHeldItem().GetComponent<ItemBehavior>().Interact();
            }
        }


    }
}
