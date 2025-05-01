using UnityEngine;
using UnityEngine.UI;

public class FearBorder : MonoBehaviour
{
    public Fear fear;
    public Image image;
    Vector4 imageColor;

    void Start()
    {
       image = GetComponent<Image>();
    }

    void Update()
    {
        imageColor = new Color(1f, 1f, 1f, 0f);
        imageColor.w = ((fear.fearMeter *.8f ) /100f);
        image.color = imageColor;
    }
}
