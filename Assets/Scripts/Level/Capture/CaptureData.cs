using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptureData", menuName = "Scriptable Objects/CaptureData")]
public class CaptureData : ScriptableObject
{

    [Header("Capture Information")]
    public Texture2D capture;
    public DateTime timestamp;
    public int score;

    [Header("Capture Flags")]
    public bool hasGhost;
    public bool hasExtraEvent;
    public int evidenceCount;
    
}
