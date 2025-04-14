using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// This script is responsible for checking the type of camera that is rendering this
/// object and applying filters if necessary
/// </summary>
public class RenderFilterChecker : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera.tag == "CameraItem")
        {
            foreach (Material mat in meshRenderer.materials)
            {
                if (mat.HasFloat("_Nightvision_Enabled"))
                {
                    mat.SetFloat("_Nightvision_Enabled", 1f);
                }
            }
        }
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        foreach (Material mat in meshRenderer.materials)
        {
            if (mat.HasFloat("_Nightvision_Enabled"))
            {
                mat.SetFloat("_Nightvision_Enabled", 0f);
            }
        }
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
