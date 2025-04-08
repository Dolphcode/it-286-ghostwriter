using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    /// The van collision box for checking if items are still in the level
    /// </summary>
    [SerializeField]
    private Collider vanBox;

    /// <summary>
    /// TEMPORARY a list of transforms representing item spawn points
    /// </summary>
    [SerializeField]
    private Transform[] itemSpawnPoints;

    // State
    List<Room>[] zones;
    List<ItemData> itemsInLevel = new List<ItemData>();
    public bool levelStarted = false;

    private void Awake()
    {
        // This will run level generation/initialization
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // Append every interactable to the capture manager
        
    }

    /// <summary>
    /// Call this function to initialize the level, leave null if using default
    /// evaluator
    /// </summary>
    /// <param name="eval">The evaluator instance to be used</param>
    public void InitializeLevel(LevelEvaluator eval)
    {
        if (eval == null) eval = levelEvaluator;
        else levelEvaluator = eval;
        zones = eval.InitializeInterior(interiorBase, 1); // should relegate this to the level loader really
        Debug.Log(zones[0].Count);
        foreach (Room r in eval.GetAllRooms())
        {
            foreach (GhostInteractable i in r.GetAllInteractables())
            {
                captureManager.AppendInteractable(i);
                captureManager.AppendCapturable(i);
            }
        }
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

        // Add this item to the capture manager's list?
        captureManager.AppendItemBehavior(behavior);
        Debug.Log(behavior.data.name);
        itemsInLevel.Add(behavior.data);
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

        // Add this ghost to the capture manager's list
        captureManager.AppendGhost(ghost);
        ghost.SetBodyType(true);
        ghost.levelManager1 = this;
        ghost.SetHuntingZone(zones[0]);
        ghost.SetGhostType(GhostType.METAPHYSICAL);
        ghost.SetDifficulty(1);
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
        Debug.Log("room index " + index.ToString());
        Debug.Log(roomList.Count);
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

    // -----------------------------------------------------------------------
    //  Level Transitions
    // -----------------------------------------------------------------------

    public void LoseLevel()
    {
        LevelLoader._Instance.LoadLobby();
    }

    public void ExitLevel()
    {
        // Call functions in the level data to create the blog entry
        // Call functions in the level data to deal with reading items
        foreach (ItemData data in itemsInLevel)
        {
            Debug.Log(data.name);
            if (vanBox.bounds.Contains(data.Behavior.transform.position))
            {
                LevelDataManager._Instance.AddItem(data.ID);
            }
            
        }
        LevelLoader._Instance.LoadLobby();
    }

    public void TriggerLoadLobby()
    {
        LevelLoader._Instance.LoadLobby();
    }
}
