using UnityEngine;
using System.IO;

public class RemoteCameraBehavior : ItemBehavior
{
    [Header("References")]
    [SerializeField]
    Camera camReference;
    [SerializeField]
    MeshRenderer renderMesh;
    Collider collider;
    Rigidbody rbody;
    [SerializeField] private AudioSource sound;
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

    public void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    public override void Interact() {}

    public void Capture(Capturable obj)
    {
        //Debug.Log(obj.name);
        //Debug.Log("Cooldown " + data.cooldowns[0].ToString() + " Cooldown Max " + data.cooldownMaxes[0].ToString());
        if (data.cooldowns[0] > 0) return;

        Debug.Log("we good with the cooldown");

        Vector3 screenpos = camReference.WorldToViewportPoint(obj.transform.position);
        // First test if it is on screen
        if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 && screenpos.z >= 0)
        {
            //Debug.Log("The thing is on screen");
            RaycastHit hit;
            
            if (Physics.Raycast(camReference.transform.position,
                (obj.transform.position - camReference.transform.position).normalized,
                out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == obj.gameObject)
            {
                //Debug.Log("I hit a " + hit.collider.name);
                //Debug.Log("I HAVE DETECTED SOMETHING! SOMETHING HAS HAPPENED");
                RenderTexture currentRT = RenderTexture.active;
                RenderTexture.active = camReference.targetTexture;
                sound.Play();
                camReference.Render();

                Texture2D image = new Texture2D(camReference.targetTexture.width, camReference.targetTexture.height);
                image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
                image.Apply();
                RenderTexture.active = currentRT;

                CaptureData data = levelManager.GetCaptureManager().CaptureImage(image, camReference);
                data.remoteCapture = true;

                // Reset cooldown
                this.data.cooldowns[0] = this.data.cooldownMaxes[0];
                /*
                var bytes = image.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/Captures/" + data.timestamp.ToShortDateString().Replace("/", "-") + "-" +
                    data.timestamp.ToLongTimeString().Replace(":", "-").Replace(" ", "-") + ".png", bytes);*/
            }
        }

        
    }
    public void CaptureHeld(CapturableObject o)
    {
        //Debug.Log(obj.name);
        //Debug.Log("Cooldown " + data.cooldowns[0].ToString() + " Cooldown Max " + data.cooldownMaxes[0].ToString());
        if (data.cooldowns[0] > 0) return;

        Debug.Log("we good with the cooldown");
        GameObject obj = o.GetCheckObject();

        Vector3 screenpos = camReference.WorldToViewportPoint(obj.transform.position);
        // First test if it is on screen
        if (screenpos.x < 1 && screenpos.x > 0 && screenpos.y > 0 && screenpos.y < 1 && screenpos.z >= 0)
        {
            //Debug.Log("The thing is on screen");
            RaycastHit hit;

            if (Physics.Raycast(camReference.transform.position,
                (obj.transform.position - camReference.transform.position).normalized,
                out hit, Mathf.Infinity, ~LayerMask.GetMask("BoundingBox")) && hit.collider.gameObject == obj.gameObject)
            {
                //Debug.Log("I hit a " + hit.collider.name);
                //Debug.Log("I HAVE DETECTED SOMETHING! SOMETHING HAS HAPPENED");
                RenderTexture currentRT = RenderTexture.active;
                RenderTexture.active = camReference.targetTexture;

                camReference.Render();

                Texture2D image = new Texture2D(camReference.targetTexture.width, camReference.targetTexture.height);
                image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
                image.Apply();
                RenderTexture.active = currentRT;

                CaptureData data = levelManager.GetCaptureManager().CaptureImage(image, camReference);
                data.remoteCapture = true;

                // Reset cooldown
                this.data.cooldowns[0] = this.data.cooldownMaxes[0];
                /*
                var bytes = image.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/Captures/" + data.timestamp.ToShortDateString().Replace("/", "-") + "-" +
                    data.timestamp.ToLongTimeString().Replace(":", "-").Replace(" ", "-") + ".png", bytes);*/
            }
        }


    }
    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
        levelManager = FindAnyObjectByType<LevelManager>();
        if (!registered)
        {
            registered = true;
            Debug.Log("Registering");
            levelManager.GetCaptureManager().RegisterEventListener(Capture);
            levelManager.GetCaptureManager().RegisterEventListener(CaptureHeld);
        }
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
            Debug.Log("Registering");
            levelManager.GetCaptureManager().RegisterEventListener(Capture);
            levelManager.GetCaptureManager().RegisterEventListener(CaptureHeld);
        }
        if (data.cooldowns[0] > 0f)
        {
            data.cooldowns[0] -= Time.deltaTime;
        }
    }
}
