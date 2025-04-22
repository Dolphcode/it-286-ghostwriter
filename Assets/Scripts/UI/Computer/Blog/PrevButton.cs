using UnityEngine;
public class prevButton : MonoBehaviour
{
    [SerializeField]
    private GameObject prevPage;
    [SerializeField]
    private GameObject currPage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (prevPage != null) prevPage.SetActive(true);
        if (currPage != null) currPage.SetActive(false);
    }
}
