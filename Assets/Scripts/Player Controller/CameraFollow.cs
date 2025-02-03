using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        /// <summary>
        /// This keeps the cameras position on the player, maintaining First Person POV
        /// </summary>
        transform.position = cameraPosition.position;
    }
}
