using System.IO;
using UnityEngine;

public class HandheldCameraBehavior : ItemBehavior
{

    [Header("References")]
    [SerializeField]
    Camera camReference;
    [SerializeField]
    MeshRenderer renderMesh;

    public void Awake()
    {
        base.Awake();
        RenderTexture tex = new RenderTexture(320, 240, 16, RenderTextureFormat.ARGB32);
        tex.useDynamicScale = true;
        tex.Create();
        camReference.targetTexture = tex;
        renderMesh.material.mainTexture = tex;
    }
    public override void Interact()
    {
        Debug.Log("snap");
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camReference.targetTexture;

        camReference.Render();

        Texture2D image = new Texture2D(camReference.targetTexture.width, camReference.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, image.width, image.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;

        var bytes = image.EncodeToPNG();
        Destroy(image);
        File.WriteAllBytes(Application.dataPath + "/Captures/test.png", bytes);

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
