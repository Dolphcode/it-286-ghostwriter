using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    /// <summary>
    /// Ghost prefab.
    /// </summary>
    [SerializeField]
    private GameObject ghostPrefab;

    ///<summary>
    ///Instantiates Ghost if prefab does not exist. Will have randomized difficulty and hunting zone. Default masc model.
    ///</summary>
    public void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab);
            Ghost ghostScript = newGhost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.SetBodyTypeF(false);
                ghostScript.SetDifficulty(Random.Range(1, 5));
            }
        }
    }
    ///<summary>
    ///Instantiates Ghost if prefab does not exist. GhostType must be PSYCHOLOGICAL, BIOLOGICAL, or METAPHYSICAL.
    ///</summary>
    public void SpawnGhost(bool isFem, GhostType type, int difficultyLevel, System.Collections.Generic.List<Room> huntingArea)
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab);
            Ghost ghostScript = newGhost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.SetBodyTypeF(isFem);
                ghostScript.SetDifficulty(difficultyLevel);
                ghostScript.SetGhostType(type);
                ghostScript.SetHuntingZone(huntingArea);
            }
        }
    }
}
