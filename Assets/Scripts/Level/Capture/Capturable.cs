using UnityEngine;

public interface Capturable
{
    /// <summary>
    /// Returns the renderer to be checked by the camera
    /// </summary>
    /// <returns>A Renderer reference</returns>
    public Renderer GetRendererCheckable();


    /// <summary>
    /// Returns a score associated with this object if it is captured
    /// </summary>
    /// <returns>An integer score for this object</returns>
    public int GetCaptureScore();
}
