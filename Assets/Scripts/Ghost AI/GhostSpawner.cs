using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    /// <summary>
    /// Ghost prefab.
    /// </summary>
    [SerializeField]
    private GameObject ghostPrefab;
    ///<summary>
    ///Instantiates Ghost if prefab does not exist. Will have randomized difficulty and hunting zone. Random between fem or masc model.
    ///</summary>
    public void SpawnGhost()
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab);
            Ghost ghostScript = newGhost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.SetDifficulty(Random.Range(1, 6));
                ghostScript.SetGhostType(Random.Range(1, 4));
            }
        }
    }
    ///<summary>
    ///Instantiates Ghost if prefab does not exist. True for bool isFem = fem model, false = masc model. GhostType variable must be PSYCHOLOGICAL, BIOLOGICAL, or METAPHYSICAL.
    ///</summary>
    public void SpawnGhost(bool isFem, GhostType type, int difficultyLevel, System.Collections.Generic.List<Room> huntingArea)
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab);
            Ghost ghostScript = newGhost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.SetDifficulty(difficultyLevel);
                ghostScript.SetGhostType(type);
                ghostScript.SetHuntingZone(huntingArea);
            }
        }
    }
    ///<summary>
    ///Instantiates Ghost if prefab does not exist. True for bool isFem = fem model, false = masc model. String for GhostType can be P, B, or M OR Psych, Bio, Meta or Phys. (did this for future beta journalism stuff)
    ///</summary>
    public void SpawnGhost(bool isFem, string type, int difficultyLevel, System.Collections.Generic.List<Room> huntingArea)
    {
        if (ghostPrefab != null)
        {
            GameObject newGhost = Instantiate(ghostPrefab);
            Ghost ghostScript = newGhost.GetComponent<Ghost>();
            if (ghostScript != null)
            {
                ghostScript.SetDifficulty(difficultyLevel);
                ghostScript.SetGhostType(type);
                ghostScript.SetHuntingZone(huntingArea);
            }
        }
    }
}
