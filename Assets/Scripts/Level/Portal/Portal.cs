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

    [SerializeField]
    private float clipThreshold = -0.01f;

    [SerializeField]
    private Vector3 clipDirection = Vector3.up;

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
                    -(linkedPosition.rotation * clipDirection).x,
                    -(linkedPosition.rotation * clipDirection).y,
                    -(linkedPosition.rotation * clipDirection).z,
                    Vector3.Dot(linkedPosition.position + clipThreshold * (linkedPosition.rotation * clipDirection), (linkedPosition.rotation * clipDirection)));
        
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
                /*
                linkedPortal.teleportOut = true;
                float ogy = other.transform.parent.transform.position.y;
                other.transform.parent.transform.position = 
                    new Vector3(linkedPortal.teleportPosition.position.x, ogy, linkedPortal.teleportPosition.position.z);
                */
                linkedPortal.teleportOut = true;

                Vector3 posInSourceSpace = transform.InverseTransformPoint(other.transform.position);
                Quaternion rotInSourceSpace = Quaternion.Inverse(transform.rotation) * other.transform.rotation;

                other.transform.parent.transform.position = linkedPosition.TransformPoint(posInSourceSpace);
                other.transform.parent.transform.rotation = linkedPosition.rotation * rotInSourceSpace;

            }
        }
    }
}
