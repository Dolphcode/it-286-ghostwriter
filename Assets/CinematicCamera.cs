using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.forward * 0.8f * Time.deltaTime;
        move.y = 0;
        transform.position += move;
    }
}
