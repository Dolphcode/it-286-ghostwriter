using UnityEngine;

public abstract class ItemBehavior : MonoBehaviour
{
    public ItemData data;
    public LevelManager levelManager;
    Collider coll;
    Rigidbody rb;


    public abstract void Unload();
    public abstract void Load(ItemData itemData);

    public abstract void Interact();

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

    }

    /// <summary>
    /// Puts item in hand of player by turning making collider a trigger and the rigidbody kinematic. Re-parents the item to Item Location
    /// </summary>
    /// <param name="itemContainer"> The location where the item will be parented.  </param>
    public virtual void PutInHand(Transform itemContainer)
    {
        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero); //Quaternion.Euler(itemData.defaultRotation); 


        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (coll != null)
        {
            coll.isTrigger = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    /// <summary>
    /// Gets rid of the parent and throws the item that is in the parents hand, while re-enabling the gravity and collider.
    /// </summary>
    public virtual void Drop()
    {
      
        transform.SetParent(null);
        
        

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);

        if (coll != null)
        {
            coll.isTrigger = false;
        }
    }
}
