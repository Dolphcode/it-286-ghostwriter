using UnityEngine;

public class RemoteCameraBehavior : ItemBehavior
{
    [Header("References")]
    [SerializeField]
    Camera camReference;
    [SerializeField]
    MeshRenderer renderMesh;

    Collider collider;
    Rigidbody rbody;

    private bool registered = false;

    public void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        RenderTexture tex = new RenderTexture(320, 240, 16, RenderTextureFormat.ARGB32);
        tex.useDynamicScale = true;
        tex.Create();
        camReference.targetTexture = tex;
        renderMesh.material.mainTexture = tex;
    }

    

    public override void Interact() {}

    public void Capture(Capturable obj)
    {

        Vector3 screenpos = camReference.WorldToViewportPoint(obj.transform.position);
        // First test if it is on screen
        if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 && screenpos.z >= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(camReference.transform.position,
                (obj.transform.position - camReference.transform.position).normalized,
                out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == obj.gameObject)
            {
                Debug.Log("I HAVE DETECTED SOMETHING! SOMETHING HAS HAPPENED");
                RenderTexture currentRT = RenderTexture.active;
                RenderTexture.active = camReference.targetTexture;

                camReference.Render();

                Texture2D image = new Texture2D(camReference.targetTexture.width, camReference.targetTexture.height);
                image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
                image.Apply();
                RenderTexture.active = currentRT;

                CaptureData data = levelManager.GetCaptureManager().CaptureImage(image, camReference);
                data.remoteCapture = true;
            }
        }

        
    }

    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
    }

    public override void Unload()
    {
        Debug.Log("Camera unloaded");
    }

    public override void PutInHand(Transform itemContainer)
    {
        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero); //Quaternion.Euler(itemData.defaultRotation); 

        itemContainer.localPosition = data.heldItemPosition;
        itemContainer.localRotation = Quaternion.Euler(data.heldItemRotation);

        if (rbody != null)
        {
            rbody.isKinematic = true;
            rbody.useGravity = false;
        }

        if (collider != null)
        {
            collider.isTrigger = true;
        }

    }

    public override void Drop()
    {
        transform.SetParent(null);

        if (rbody != null)
        {
            rbody.isKinematic = false;
            rbody.useGravity = true;
        }

        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!registered)
        {
            registered = true;
            levelManager.GetCaptureManager().RegisterEventListener(Capture);    
        }
    }
}
