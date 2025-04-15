using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Image staminaBar;
    public float stamina;
    public float maxStamina;
    public float stamChargeRate;
    public float runCost;
    private Coroutine recharging;
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
        MyInput();
        SpeedControl();
        rb.linearDamping = groundDrag;

        //Left Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            StopCoroutine(recharging);
            if (stamina > 0)
            { 
            Sprint();
            stamina -= runCost * Time.deltaTime;
            
            }
            
            if (stamina <= 0)
            {
                stamina = 0;
                Walk();

            }
            staminaBar.fillAmount = stamina / maxStamina;
        } 
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walk();

            if (recharging != null)
            {
                StopCoroutine(recharging);
            }

            recharging = StartCoroutine(RechargeStamina());

        }


        
    }

    
    /// <summary>
    /// Starts recharging the players stamina after they stop running for 1 second.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (stamina < maxStamina)
        {
            stamina += stamChargeRate / 10f;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            staminaBar.fillAmount = stamina / maxStamina;
            yield return new WaitForSeconds(.1f);
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