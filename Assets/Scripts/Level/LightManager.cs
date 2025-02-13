using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The base GhostInteractable interface which must be implemented by
/// all objects that the ghost can interact with in the level.
/// </summary>
public class LightManager : MonoBehaviour
{
    /// <summary>
    /// Stores the flashlight color
    /// </summary>
    [SerializeField]
    private Color flashlightColor = (new Color(255f/255f, 249f/255f, 144f/255f));
    /// <summary>
    /// Set the flashlight's color
    /// </summary>
    /// <param name="flashlightColor">The flashlight spotlight color</param>
    public void SetFlashlightColor(Color flashlightColor) { this.flashlightColor = flashlightColor; }

    /// <summary>
    /// Stores whether the flashlight is on or not
    /// </summary>
    [SerializeField]
    private bool flashlightEnabled = false;
    /// <summary>
    /// Set whether the flashlight is on or not
    /// </summary>
    /// <param name="flashlightEnabled">The state of the flashlight, true if on and false if not</param>
    public void SetFlashlightState(bool flashlightEnabled) { this.flashlightEnabled = flashlightEnabled; }

    /// <summary>
    /// Stores the flashlight position
    /// </summary>
    [SerializeField]
    private Vector3 flashlightPosition = Vector3.zero;

    /// <summary>
    /// Stores the flashlight direction
    /// </summary>
    [SerializeField]
    private Vector3 flashlightDirection = Vector3.forward;

    /// <summary>
    /// Set the flashlight's spatial information
    /// </summary>
    /// <param name="pos">Set the position of the flashlight origin</param>
    /// <param name="dir">Set the direction of the flashlight</param>
    public void SetFlashlightPoint(Vector3 pos, Vector3 dir)
    {
        flashlightPosition = pos;
        flashlightDirection = dir;
    }

    private void Awake()
    {
        // Initialize Flashlight
        Shader.SetGlobalFloat("_Flashlight_On", flashlightEnabled ? 1.0f : 0.0f);
        Shader.SetGlobalColor("_LightColor", flashlightColor);
        //Shader.SetGlobalVector("_World_Space_Light_Position", flashlightPosition);
        //Shader.SetGlobalVector("_Spotlight_Direction", flashlightDirection);
    }

    private void Update()
    {
        // Update every frame
        Shader.SetGlobalFloat("_Flashlight_On", flashlightEnabled ? 1.0f : 0.0f);
        Shader.SetGlobalColor("_LightColor", flashlightColor);
        //Shader.SetGlobalVector("_World_Space_Light_Position", flashlightPosition);
        //Shader.SetGlobalVector("_Spotlight_Direction", flashlightDirection);
    }
}
