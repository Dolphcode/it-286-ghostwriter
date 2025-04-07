using UnityEngine;
using UnityEngine.Events;

public class ButtonAction : PlayerInteractable
{
    public UnityEvent _OnClick;

    public override void interact()
    {
        //Button_clicked();
        _OnClick.Invoke();
    }


}
