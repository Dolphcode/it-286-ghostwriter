using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindButton : MonoBehaviour
{
    [SerializeField] string controlAction;
    

    private InputAction m_Action; // Reference to an action to rebind.
    public TextMeshProUGUI m_DisplayText; // Text in UI that receives the binding display string.

    private InputActionRebindingExtensions.RebindingOperation m_Operation;

    private void Awake()
    {
        m_Action = InputSystem.actions.FindAction(controlAction);
        int idx = m_Action.bindings[0].effectivePath.IndexOf("/");
        m_DisplayText.text = m_Action.bindings[0].effectivePath.Substring(idx + 1).ToUpper();
    }

    public void Update()
    {
        int idx = m_Action.bindings[0].effectivePath.IndexOf("/");
        m_DisplayText.text = m_Action.bindings[0].effectivePath.Substring(idx + 1).ToUpper();
    }

    public void Rebind()
    {
        Debug.Log(!InputSystem.actions.FindActionMap("Player").enabled);
        if (!InputSystem.actions.FindActionMap("Player").enabled) return;
        InputSystem.actions.FindActionMap("Player").Disable();
        m_Operation = m_Action.PerformInteractiveRebinding()
            .OnComplete(operation => RebindCompleted());
        m_Operation.Start();
    }

    void RebindCompleted()
    {
        m_Operation.Dispose();
        int idx = m_Action.bindings[0].effectivePath.IndexOf("/");
        string newBinding = m_Action.bindings[0].effectivePath.Substring(idx + 1).ToUpper();
        m_DisplayText.text = newBinding;
        InputSystem.actions.FindActionMap("Player").Enable();
        SaveDataManager._Instance.SaveData(LevelDataManager._Instance);
    }
}
