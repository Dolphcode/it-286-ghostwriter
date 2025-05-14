using DG.Tweening;
using UnityEngine;

public class DrawerInteractable : GhostInteractable
{

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private GameObject topDrawer;
    [SerializeField] private GameObject middleDrawer;
    [SerializeField] private GameObject bottomDrawer;

    [SerializeField] private bool openTop;
    [SerializeField] private bool openMiddle;
    [SerializeField] private bool openBottom;

    [SerializeField] private float closedY;
    [SerializeField] private float openY;

    private AudioSource source;

    private bool topOpen = false, middleOpen = false, bottomOpen = false;
    public override void interact()
    {
        int randomnum = Random.Range(0, 2);
        if (randomnum == 0)
        {
            ToggleTop();
        } else if (randomnum == 1)
        {
            ToggleMiddle();
        } else
        {
            ToggleBottom();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ToggleTop()
    {
        if (!topOpen) topDrawer.transform.DOLocalMoveZ(openY, 1f).SetEase(easeCurve);
        else topDrawer.transform.DOLocalMoveZ(closedY, 1f).SetEase(easeCurve);
        topOpen = !topOpen;
        source.Play();
    }

    public void ToggleMiddle()
    {
        if (!middleOpen) middleDrawer.transform.DOLocalMoveZ(openY, 1f).SetEase(easeCurve);
        else middleDrawer.transform.DOLocalMoveZ(closedY, 1f).SetEase(easeCurve);
        middleOpen = !middleOpen;
        source.Play();
    }

    public void ToggleBottom()
    {
        if (!bottomOpen) bottomDrawer.transform.DOLocalMoveZ(openY, 1f).SetEase(easeCurve);
        else bottomDrawer.transform.DOLocalMoveZ(closedY, 1f).SetEase(easeCurve);
        bottomOpen = !bottomOpen;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // For debugging manually do this
        if (openTop)
        {
            openTop = false;
            ToggleTop();
        }
        if (openMiddle)
        {
            openMiddle = false;
            ToggleMiddle();
        }
        if (openBottom)
        {
            openBottom = false;
            ToggleBottom();
        }

    }
}
