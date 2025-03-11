using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Movement
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Check if player is on ground
    /// </summary>
    /// 
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isOnGround;
    

    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void Update()
    {
        ///<summary>
        ///Ground Check
        ///</summary>
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        MyInput();
        SpeedControl();
        if (isOnGround)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    private void FixedUpdate()
    {
        ///<summary>
        ///Uses MovePLayer() function to check if an input is pressed
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