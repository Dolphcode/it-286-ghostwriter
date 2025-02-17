using UnityEditor.Experimental.GraphView;
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
    public Transform itemContainer;
    public RaycastHit lookingAt;
    bool isHolding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ///<summary>
        /// Makes the cursor invisible & locks it in the window
        /// </summary>

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        isHolding = false;
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

        
        if (Input.GetKeyDown(KeyCode.E) && (isHolding == false))
        {
            Physics.Raycast(transform.position, transform.forward, out lookingAt, 5f);
            if (lookingAt.collider != null)
            {
                if (lookingAt.collider.GetComponent<ItemBehavior>() != null)
                {
                    lookingAt.collider.GetComponent<ItemBehavior>().PutInHand(itemContainer);
                    isHolding = true;
                }
            }
        }
            Debug.Log("looking At " + lookingAt.collider);

        if (Input.GetKeyDown(KeyCode.G) && (isHolding == true))
            {
                itemContainer.GetChild(0).GetComponent<ItemBehavior>().Drop();
                isHolding = false;

            }
        
    }
}
