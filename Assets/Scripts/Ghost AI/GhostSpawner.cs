using Unity.VisualScripting;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    /// <summary>
    /// Ghost prefab.
    /// </summary>
    [SerializeField]
    private GameObject ghostPrefab;

    private void Start()
    {
        SpawnGhost();
    }
    ///<summary>
    ///Instantiates Ghost if prefab does not exist.
    ///</summary>
    public void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            Instantiate(ghostPrefab);
        }
    }
}
