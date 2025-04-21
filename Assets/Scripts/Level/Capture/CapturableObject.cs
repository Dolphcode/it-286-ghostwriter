using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CapturableObject : ScriptableObject
{
    /// <summary>
    /// A UnityEvent for triggering the capture of a remote camera
    /// </summary>
    protected UnityEvent<Capturable> m_TriggerCapture;
    public bool HasTriggerCaptureEvent() { return m_TriggerCapture != null; }

    public void AddTriggerListener(UnityAction<Capturable> callback)
    {
        Debug.Log("added listener");
        m_TriggerCapture.AddListener(callback);
    }

    /// <summary>
    /// Get the gameobject used for a raycast test for this capturable
    /// </summary>
    /// <returns>A gameobject which should be used to test if the raycast is hitting the object</returns>
    public abstract GameObject GetCheckObject();

    /// <summary>
    /// Returns a score associated with this object if it is captured
    /// </summary>
    /// <param name="rayProp">The proportion of rays in a raycast grid hitting this object</param>
    /// <param name="data">A reference to the data object to trigger flags</param>
    /// <returns>An integer score for this object</returns>
    public abstract int GetCaptureScore(float rayProp, CaptureData data);
}
