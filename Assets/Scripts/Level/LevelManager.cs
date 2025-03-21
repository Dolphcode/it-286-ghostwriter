using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The LevelManager script is responsible for initializing the level, ghost,
/// player, e.t.c. when the level is first loaded. It will also hold
/// references to level components to make them easily accessible.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Level Gen")]
    /// <summary>
    /// An implementation of the Strategy pattern for level generation and 
    /// determining which room a player is in.
    /// </summary>
    [SerializeField]
    private LevelEvaluator levelEvaluator;


    [Header("World Items")]
    /// <summary>
    /// The scene's Light manager, which can be used to make calls to change
    /// the state of the flashlight. Also works in engine
    /// </summary>
    [SerializeField]
    private LightManager lightManager;
    public LightManager GetLightManager() { return lightManager; }

    /// <summary>
    /// The scene's Capture manage responsible for storing capture data
    /// </summary>
    [SerializeField]
    private CaptureManager captureManager;
    public CaptureManager GetCaptureManager() { return captureManager; }

    /// <summary>
    /// The root object of the interior of the level in the scene hierarchy.
    /// </summary>
    [SerializeField]
    private GameObject interiorBase;

    /// <summary>
    /// A reference to the player object. Must be assigned in editor
    /// </summary>
    [SerializeField]
    private PlayerMovement player;
    public PlayerMovement GetPlayerController() { return player; }

    /// <summary>
    /// A list of ghosts, is initialized on start
    /// </summary>
    [SerializeField]
    private List<Ghost> ghosts;
    public List<Ghost> GetGhostList() { return ghosts; }

    /// <summary>
    /// TEMPORARY a list of transforms representing item spawn points
    /// </summary>
    [SerializeField]
    private Transform[] itemSpawnPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // This will run level generation/initialization
        levelEvaluator.InitializeInterior(interiorBase, 1); // should relegate this to the level loader really

        // Instantiate and initialize the ghost objects

        // TODO: Give the player a reference to this object
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(IsPlayerInRoom(levelEvaluator.GetAllRooms()[0]));
    }

    // -----------------------------------------------------------------------
    //  Level Initialization
    // -----------------------------------------------------------------------

    /// <summary>
    /// Adds an item behavior object to the world on load
    /// </summary>
    /// <param name="behavior">The object to be added to the world</param>
    /// <param name="spawnPointNumber">The spawn position</param>
    public void AddItemToWorld(ItemBehavior behavior, int spawnPointNumber)
    {
        // Add the item to the world
        behavior.transform.SetParent(transform);

        behavior.transform.position = itemSpawnPoints[spawnPointNumber].position;
    }

    public void AddGhostToWorld(Ghost ghost)
    {
        // TODO: Move Ghost initialization code to an Init function
        // Put the ghost in the world
        ghosts = new List<Ghost>();
        ghosts.Add(ghost);

        // Need to flesh this out more
        ghost.transform.SetParent(transform);

        ghost.SetPlayer(player.transform);
    }


    // -----------------------------------------------------------------------
    //  Exposing information stored in the Level Evaluator
    // -----------------------------------------------------------------------

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

    // -----------------------------------------------------------------------
    //  Functions relating to room checking
    // -----------------------------------------------------------------------

    /// <summary>
    /// Checks if the ghost is in the same room as a given position. Calls GetRoomFromPosition
    /// and returns true if any ghosts are currently haunting that room.
    /// </summary>
    /// <param name="position">Position to be tested</param>
    /// <returns>True or False depending on if the ghost is in a given room</returns>
    public bool IsGhostInRoom(Vector3 position)
    {
        Room posRoom = GetRoomFromPosition(position);
        return IsGhostInRoom(posRoom);
    }

    /// <summary>
    /// Checks if the ghost is in the same room as a given position. An overload that takes the
    /// room object
    /// </summary>
    /// <param name="room">Room to be tested</param>
    /// <returns>True or False depending on if the ghost is in a given room</returns>
    public bool IsGhostInRoom(Room room)
    {
        // TODO: Once ghost is implemented actually check this
        // For now returns true if the player is in room 0
        foreach (Ghost ghost in ghosts)
        {
            if (ghost.GetGhostRoom() == room)
            {
                Debug.Log("there's a ghost here!");
                return true;
            } else
            {
                Debug.Log("no ghost here!");
            }
        }
        return false;
    }

    /// <summary>
    /// Gets a list of ghosts in the current room given a point
    /// </summary>
    /// <param name="position">The position to test what room</param>
    /// <returns>A List\<Ghost\> of ghosts in the room </returns>
    public List<Ghost> GetGhostsInRoom(Vector3 position)
    {
        Room posRoom = GetRoomFromPosition(position);
        return GetGhostsInRoom(posRoom);
    }

    /// <summary>
    /// Gets a list of ghosts in the current room given a Room object
    /// </summary>
    /// <param name="room">The room to be searched</param>
    /// <returns>A List\<Ghost\> of ghosts in the room </returns>
    public List<Ghost> GetGhostsInRoom(Room room)
    {
        List<Ghost> ghostsInRoom = new List<Ghost>();

        foreach (Ghost ghost in ghosts)
        {
            if (ghost.GetGhostRoom() == room)
            {
                ghostsInRoom.Append(ghost);
            }
        }
        return ghostsInRoom;
    }

    /// <summary>
    /// Returns the number of ghosts in the same room as the room corresponding to a specified
    /// position. Calls GetRoomFromPosition and iterates over all ghosts to check if their haunting 
    /// room is this room.
    /// </summary>
    /// <param name="position">The position being tested</param>
    /// <returns>The number of ghosts in the room</returns>
    public int NumGhostsInRoom(Vector3 position)
    {
        Room room = GetRoomFromPosition(position);
        return NumGhostsInRoom(room);
    }

    /// <summary>
    /// Returns the number of ghosts in the same room as the room provided. An overload that
    /// takes a room instead of a position
    /// </summary>
    /// <param name="room">The room object</param>
    /// <returns>The number of ghosts in the room</returns>
    public int NumGhostsInRoom(Room room)
    {
        int numGhosts = 0;
        foreach (Ghost ghost in ghosts)
        {
            if (ghost.GetGhostRoom() == room)
            {
                numGhosts++;
            }
        }
        return numGhosts;
    }

    /// <summary>
    /// Checks if the player is in a given room
    /// </summary>
    /// <param name="room">The room to be tested</param>
    /// <returns>True if the player is in the room, False if not</returns>
    public bool IsPlayerInRoom(Room room)
    {
        Room playerRoom = GetRoomFromPosition(player.transform.position);
        return room == playerRoom;
    }
}
