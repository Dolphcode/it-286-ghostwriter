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

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public override void interact()
    {
        if (interactable) {
            transform.DORotate(new Vector3(-70, transform.eulerAngles.y, transform.eulerAngles.z), 3).SetEase(testCurve);
            interactable = false;
            cooldown = 4f;
            source.Play();
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
            m_TriggerCapture.Invoke(this);
            source.Play();
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
