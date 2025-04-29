using DG.Tweening;
using UnityEngine;

public class DoorInteractable : GhostInteractable
{

    [SerializeField] private Transform door;
    [SerializeField] private AudioSource sound;
    private bool open;

    public void ToggleDoor()
    {
        if (open) door.DORotate(new Vector3(door.eulerAngles.x, door.eulerAngles.y, -90f), 1f);
        else door.DORotate(new Vector3(door.eulerAngles.x, door.eulerAngles.y, 90f), 1f);
        open = !open;
        sound.Play();
    }
    
    public override void interact()
    {
        ToggleDoor();
    }
}
