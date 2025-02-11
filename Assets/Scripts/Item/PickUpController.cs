using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, itemContainer, cam;
    public Transform itemLocation;
    public float pickUpRange;
    public float dropForwardForce;
    public Transform orientation;

    public bool equipped;
    public static bool inHand;

    private void Start()
    {
        if (!equipped)
        {
            ///Disable script for Tool if not able to be used on ground
                //ItemScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        if (equipped)
        {
            ///Enable script for equipped Tool
                //ItemScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            inHand = true;

        }
    }
    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !inHand)
        {
            PickUp();
        }

        if (equipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
        if (inHand)
        {
            transform.position = itemLocation.position;
            transform.rotation = Quaternion.LookRotation(orientation.rotation * Vector3.forward);
        }
      }
    private void PickUp()
    {
        equipped = true;
        inHand = true;

        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;

        ///Enable script for Tool
        //ItemScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        inHand = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.linearVelocity = player.GetComponent<Rigidbody>().linearVelocity;

        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
  
        ///Disable script for Tool if not able to be used on ground
        //ItemScript.enabled = false;

    }
}
