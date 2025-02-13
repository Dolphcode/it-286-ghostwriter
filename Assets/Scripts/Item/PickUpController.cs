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
    CamControl camera;
    public RaycastHit lookingAt;
    [SerializeField]
    public static bool equipped;
    [SerializeField]
    public static bool inHand;
    public static GameObject itemHeld;
    private void Start()
    {
        camera = FindAnyObjectByType<CamControl>();
        itemHeld = null;
        if (!equipped)
        {
            ///Disable script for Tool if not able to be used on ground
                //ItemScript.enabled = false;
            camera.lookingAt.rigidbody.isKinematic = false;
            camera.lookingAt.collider.isTrigger = false;
        }

        if (equipped)
        {
            ///Enable script for equipped Tool
                //ItemScript.enabled = true;
            camera.lookingAt.rigidbody.isKinematic = true;
            camera.lookingAt.collider.isTrigger = true;
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
            //transform.position = itemLocation.position;
            camera.lookingAt.transform.localPosition = Vector3.zero;
            camera.lookingAt.transform.localRotation = Quaternion.Euler(Vector3.zero);
            //transform.rotation = Quaternion.LookRotation(orientation.rotation * Vector3.forward, orientation.rotation * Vector3.up);
        }
      }
    private void PickUp()
    {
        equipped = true;
        inHand = true;

        itemHeld = lookingAt.transform.gameObject;

        camera.lookingAt.transform.SetParent(itemContainer);
        camera.lookingAt.transform.localPosition = Vector3.zero;
        camera.lookingAt.transform.localRotation = Quaternion.Euler(Vector3.zero);

        camera.lookingAt.transform.localScale = Vector3.one;
        
        camera.lookingAt.rigidbody.isKinematic = true;
        camera.lookingAt.collider.isTrigger = true;

        ///Enable script for Tool
        //ItemScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        inHand = false;

        itemHeld.transform.SetParent(null);
        itemHeld.GetComponent<Rigidbody>().isKinematic = false;
        itemHeld.GetComponent<Collider>().isTrigger = false;

        itemHeld.GetComponent<Rigidbody>().linearVelocity = player.GetComponent<Rigidbody>().linearVelocity;

        itemHeld.GetComponent<Rigidbody>().AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        itemHeld = null;
        ///Disable script for Tool if not able to be used on ground
        //ItemScript.enabled = false;

    }
}
