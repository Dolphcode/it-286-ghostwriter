using UnityEngine;

public class RemoteCameraBehavior : ItemBehavior
{
    [Header("References")]
    [SerializeField]
    Camera camReference;
    [SerializeField]
    MeshRenderer renderMesh;

    Collider coll;
    Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        RenderTexture tex = new RenderTexture(320, 240, 16, RenderTextureFormat.ARGB32);
        tex.useDynamicScale = true;
        tex.Create();
        camReference.targetTexture = tex;
        renderMesh.material.mainTexture = tex;
    }

    public void Start()
    {
        levelManager.GetCaptureManager().RegisterEventListener(Capture);
    }

    public override void Interact() {}

    public void Capture()
    {
        Debug.Log("I HAVE DETECTED SOMETHING! SOMETHING HAS HAPPENED");
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camReference.targetTexture;

        camReference.Render();

        Texture2D image = new Texture2D(camReference.targetTexture.width, camReference.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;

        levelManager.GetCaptureManager().CaptureImage(image, camReference);
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

    public override void Drop()
    {
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (coll != null)
        {
            coll.isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
