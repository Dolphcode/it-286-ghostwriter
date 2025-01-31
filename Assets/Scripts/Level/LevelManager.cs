using UnityEngine;

/// <summary>
/// The LevelManager script is responsible for initializing the level, ghost,
/// player, e.t.c. when the level is first loaded. It will also hold
/// references to level components to make them easily accessible.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// An implementation of the Strategy pattern for level generation and 
    /// determining which room a player is in.
    /// </summary>
    [SerializeField]
    public LevelEvaluator levelEvaluator { get; private set; }

    /// <summary>
    /// The root object of the interior of the level in the scene hierarchy.
    /// </summary>
    [SerializeField]
    private GameObject interiorBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This will run level generation/initialization
        levelEvaluator.GenerateInterior(interiorBase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
