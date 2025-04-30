using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlogPage", menuName = "Scriptable Objects/BlogSaveData/BlogPage")]
public class BlogPage : ScriptableObject
{   
    public BlogSaveData blog;
    public string title;    
    public string text1;
    public string text2;
    public List<string> selectedAnswers;
    public Texture2D image;
    public CaptureData imageData;
    public int pageNum;
}