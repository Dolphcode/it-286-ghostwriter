using UnityEngine;

public abstract class ItemBehavior : MonoBehaviour
{
    public ItemData data;
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
    public void PutInHand(Transform itemContainer)
    {
        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        transform.localScale = Vector3.one;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (coll != null)
        {
            coll.isTrigger = true;
        }

    }
    
    public void Drop()
    {
        

        rb.linearVelocity = transform.parent.GetComponent<Rigidbody>().linearVelocity;
        transform.SetParent(null);
        rb.AddForce(transform.forward * 100, ForceMode.Impulse);


        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (coll != null)
        {
            coll.isTrigger = false;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
