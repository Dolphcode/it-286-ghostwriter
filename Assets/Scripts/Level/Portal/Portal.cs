using UnityEngine;
using static Unity.VisualScripting.Member;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Portal linkedPortal;

    [SerializeField]
    private Transform linkedPosition;

    [SerializeField]
    private Transform teleportPosition;

    [SerializeField]
    private Camera portalCamera;

    [SerializeField]
    private Camera mainCamera;

    private bool teleportOut = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void LateUpdate()
    {
        Vector3 cameraPositionInSourceSpace = transform.InverseTransformPoint(mainCamera.transform.position);
        Quaternion cameraRotationInSourceSpace = Quaternion.Inverse(transform.rotation) * mainCamera.transform.rotation;

        portalCamera.transform.position = linkedPosition.TransformPoint(cameraPositionInSourceSpace);
        portalCamera.transform.rotation = linkedPosition.rotation * cameraRotationInSourceSpace;
        
        Vector4 clipPlaneWorldSpace =
                new Vector4(
                    -linkedPosition.up.x,
                    -linkedPosition.up.y,
                    -linkedPosition.up.z,
                    Vector3.Dot(linkedPosition.position - 0.01f * linkedPosition.up, linkedPosition.up));
        
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        
        portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        

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
                float ogy = other.transform.parent.transform.position.y;
                other.transform.parent.transform.position = 
                    new Vector3(linkedPortal.teleportPosition.position.x, ogy, linkedPortal.teleportPosition.position.z);

            }
        }
    }
}
