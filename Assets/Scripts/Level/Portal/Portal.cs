using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Portal linkedPortal;

    [SerializeField]
    private Transform teleportPosition;

    [SerializeField]
    private Transform avatar;

    [SerializeField]
    private Transform mainCamera;

    private bool teleportOut = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Player")
        {
            if (teleportOut)
            {
                teleportOut = false;
            } else
            {
                linkedPortal.teleportOut = true;
                other.transform.parent.transform.position = linkedPortal.teleportPosition.position;
            }
        }
    }
}
