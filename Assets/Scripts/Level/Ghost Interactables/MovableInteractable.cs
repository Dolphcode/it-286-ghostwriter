using DG.Tweening;
using UnityEngine;

public class MovableInteractable : GhostInteractable
{
    [SerializeField]
    private bool rock;

    [SerializeField]
    private AnimationCurve testCurve;

    [SerializeField]
    private float cooldown = 0f;

    public override void interact()
    {
        if (interactable) {
            transform.DORotate(new Vector3(-70, transform.eulerAngles.y, transform.eulerAngles.z), 3).SetEase(testCurve);
            interactable = false;
            cooldown = 4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rock)
        {
            rock = false;
            transform.DORotate(new Vector3(-70, transform.eulerAngles.y, transform.eulerAngles.z), 3).SetEase(testCurve);
            interactable = false;
            cooldown = 4f;
        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        } else
        {
            interactable = true;
        }
    }
}
