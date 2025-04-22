using UnityEngine;
public class nextButton : MonoBehaviour
{
    [SerializeField]
    private GameObject currPage;
    [SerializeField]
    private GameObject nextPage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currPage != null) currPage.SetActive(false);
        if (nextPage != null) nextPage.SetActive(true);
    }
}
