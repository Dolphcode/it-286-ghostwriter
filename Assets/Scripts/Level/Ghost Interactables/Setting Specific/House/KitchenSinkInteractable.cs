using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KitchenSinkInteractable : GhostInteractable
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private bool currentState;

    private void Awake()
    {
        m_TriggerCapture = new UnityEvent<Capturable>();
        anim.SetBool("Running", currentState);
    }

    public void Update()
    {
        anim.SetBool("Running", currentState);
    }

    /// <summary>
    /// Call this function to switch the light's state
    /// object.
    /// </summary>
    public override void interact()
    {
        if (!interactable) return;
        currentState = !currentState;
        anim.SetBool("Running", currentState);
    }

    public void ChangeState()
    {
        currentState = !currentState;
        anim.SetBool("Running", currentState);
    }
}
