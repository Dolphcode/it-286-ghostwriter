using System.Collections.Generic;
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
    private LevelEvaluator levelEvaluator;

    /// <summary>
    /// The root object of the interior of the level in the scene hierarchy.
    /// </summary>
    [SerializeField]
    private GameObject interiorBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This will run level generation/initialization
        levelEvaluator.InitializeInterior(interiorBase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Pick a random room from the list of all rooms in a level
    /// </summary>
    /// <returns>A Room object corresponding to the correct room</returns>
    public Room SelectRandomRoom()
    {
        List<Room> roomList = levelEvaluator.GetAllRooms();
        int index = Random.Range(0, roomList.Count);
        return roomList[index];
    }

    /// <summary>
    /// Gives all of the rooms in a level
    /// </summary>
    /// <returns>A List of room objects</returns>
    public List<Room> GetAllRooms()
    {
        return levelEvaluator.GetAllRooms();
    }

    /// <summary>
    /// Determines which room a point is in. Essentially calls levelEvaluator.GetRoomFromPosition()
    /// </summary>
    /// <param name="position">Point position</param>
    /// <returns>The corresponding Room object</returns>
    public Room GetRoomFromPosition(Vector3 position)
    {
        return levelEvaluator.GetRoomFromPosition(position);
    }
}
