using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Movement Speed (Default = 3)
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Check if player is on ground
    /// </summary>
    /// 
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;

    private float timeSprinting;
    private bool sprintCooldown;

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Mathf.Clamp(timeSprinting, 0,5);
        timeSprinting = 0;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        rb.linearDamping = groundDrag;

        //Left Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //if (timeSprinting <= 5 && !sprintCooldown)
            //{ 
                Sprint();
               // timeSprinting += Time.deltaTime;
            //}
           /* else
            {
                Walk();
                timeSprinting -= Time.deltaTime;
                if (timeSprinting == 0)
                {
                    sprintCooldown = false;
                }
                else
                {
                    sprintCooldown = true;
                }
            }
           */
        }

        else
        {
            Walk();
           // timeSprinting -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        ///<summary>
        ///Uses MovePlayer() function to check if an input is pressed
        ///so that the player is moved 
        ///</summary>
        MovePlayer();
    }

    private void MyInput()
    {
        ///<summary>
        /// Reads the players Input and assigns it to a variable
        /// </summary>
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }
    private void Sprint()
    {
        moveSpeed = 4.5f;
    }

    private void Walk()
    {
        moveSpeed = 3f;
    }
    private void MovePlayer()
    {
        ///<summary>
        ///Finds the direction the player is moving
        ///</summary>
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f,rb.linearVelocity.z);
        
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x,rb.linearVelocity.y, limitedVel.z);
        }
    }

}