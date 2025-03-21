using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelLoadscreenData", menuName = "Scriptable Objects/LevelLoadscreenData")]
public class LevelLoadscreenData : ScriptableObject
{
    public string levelName;
    public string loadingDescription;
    public Sprite backgroundImage;
}
