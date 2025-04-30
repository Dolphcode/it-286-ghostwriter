using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlogPage : MonoBehaviour
{   public BlogSaveData blog { get; set; }
    public string title { get; set; }
    public string text1 { get; set; }
    public string text2 { get; set; }
    public List<string> selectedAnswers { get; set; }
    public Texture2D image { get; set; }
    public CaptureData imageData { get; set; }
    public int pageNum { get; set; }
}