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
    float xRotation;
    float yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ///<summary>
        /// Makes the cursor invisible & locks it in the window
        /// </summary>

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        ///<summary>
        ///Keeps the camera from going all the way around
        /// </summary>
        xRotation = Mathf.Clamp(xRotation,-90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
