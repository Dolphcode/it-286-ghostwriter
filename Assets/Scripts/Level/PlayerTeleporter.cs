using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Transform tpos;

    public void Teleport()
    {
        player.transform.position = tpos.position;
    }
}
