using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

///<summary>
/// Ghost Types dropdown.
/// </summary>
public enum GhostType
{
    PSYCHOLOGICAL, BIOLOGICAL, METAPHYSICAL
}
public class Ghost : MonoBehaviour
{
    ///<summary>
    ///Target the ghost is chasing.
    ///</summary>
    [SerializeField]
    private Transform player;
    /// <summary>
    ///Amount of times a ghost can be provoked before entering Hunting Mode.
    ///</summary>
    [SerializeField]
    private int aggressionThreshold;
    ///<summary>
    ///Amount of times a ghost has been provoked.
    ///</summary>
    [SerializeField]
    private int aggression;
    ///<summary>
    ///Aggression multiplier of ghost. Lowers difficulty based on multiplier.
    ///</summary>
    private int aggressionMultiplier;
    ///<summary>
    ///Difficulty of ghost (lower level = higher aggression threshold, slower)
    ///</summary>
    [SerializeField]
    private int difficultyLevel;
    /// <summary>
    /// Ghost's type.
    /// </summary>
    [SerializeField]
    private GhostType type;
    ///<summary>
    ///Ghost type Psychological (more erratic behavior, speed changes, many interactions).
    ///EMF changes from 1,5;
    ///</summary>
    private bool psychologicalType;
    ///<summary>
    ///Ghost type Biological (died a natural death - not as fast, hard to aggro).
    ///EMF: 1
    ///</summary>
    private bool biologicalType;
    ///<summary>
    ///Ghost type Metaphysical (died but spiritually - aggression threshold lowers the more you aggro them. has a lot more interactions/tries to communicate with player more? maybe triggers a certain tool).
    ///EMF: 5
    ///</summary>
    private bool metaphysicalType;
    ///<summary>
    ///String containting name of ghost type.
    ///</summary>
    private string typeName;
    ///<summary>
    ///EMF variable.
    ///</summary>
    [SerializeField]
    private int emf;
    ///<summary>
    ///Aggression threshold/5.
    ///</summary>
    private int emfLevel;
    ///<summary>
    ///Room the ghost is currently in. Initialized during start to be random room.
    ///</summary>
    [SerializeField]
    private Room currentRoom;
    ///<summary>
    ///NavMesh.
    ///</summary>
    [SerializeField]
    private NavMeshAgent agent;
    ///<summary>
    ///Movement speed of ghost.
    ///</summary>
    [SerializeField]
    private float moveSpeed = 1f;
    ///<summary>
    ///Indication of ghost being in Hunting Mode. 
    ///</summary>
    [SerializeField]
    private bool huntingMode;
    ///<summary>
    ///List of rooms that the ghost can access.
    ///</summary>
    [SerializeField]
    private System.Collections.Generic.List<Room> huntingZone;
    ///<summary>
    ///Level manager.
    ///</summary>
    [SerializeField]
    private LevelManager levelManager1;
    private float aggroTimer = 0f;
    private float interactTimer = 0f;
    private float teleportTimer = 0f;
    private float huntingTimer = 0f; 
    //<summary>
    //Sets ghost position to room.
    //</summary>
    private void SetGhostPosition(Transform spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        Physics.SyncTransforms();
        Debug.Log("GHOST POSITION CHANGE");
    }
    ///<summary>
    ///Will randomize which interaction happens.
    ///</summary>
    ///
    private void RandomGhostInteraction()
    {
        Debug.Log("OBJECT INTERACT");
        //random chance of interact happening
        bool doesInteract = Random.Range(0, 2) == 0;
        int randInteract = Random.Range(0, currentRoom.filterInteractables<RoomLightInteractable>().Count);
        if (doesInteract)
        {
            currentRoom.filterInteractables<RoomLightInteractable>()[randInteract].interact();
        }
    }
    // Increases aggresssion
    public void IncreaseAggression()
    {
        aggression++;
    }
    //<summary>
    //Ghost will switch locations rooms and randomly depending on aggression level.
    //</summary>
    private void Roam()
    {
        if (aggression < GetEmfLevel() && emf < 5)
        {
            emf++;
        }
        teleportTimer += Time.deltaTime;
        interactTimer += Time.deltaTime;
        aggroTimer += Time.deltaTime;
        if (levelManager1.IsPlayerInRoom(currentRoom))
        {
            if (aggroTimer >= 1f)
            {
                aggression++;
                aggroTimer = 0f;
            }
        }
        // Ghost is not visible when in passive.
        //GetComponent<Renderer>().enabled = false;
        // If aggression less than half full game is slightly harder
        if (aggression < aggressionThreshold / 2)
        {
            //supposed to be 90 im debugging out
            if (teleportTimer >= 10f)
            {
                currentRoom = currentRoom.selectRandomAdjacentRoom(); 
                SetGhostPosition(currentRoom.selectRandomSpawnPoint());
                teleportTimer = 0f;
            }
            bool interactBool = Random.value > 0.75f;
            //supposed to be 180 im debugging out
            if (interactTimer >= 10f)
            {
                RandomGhostInteraction();
                interactTimer = 0f;
            }
        }
        // When ghost is in second half of aggression threshold
        else if (aggression < aggressionThreshold)
        {
            //supposed to be 50
            if (teleportTimer >= 20f)
            {
                currentRoom = currentRoom.selectRandomAdjacentRoom();
                SetGhostPosition(currentRoom.selectRandomSpawnPoint());

                teleportTimer = 0f;
            }
            //supposed to be 100
            if (interactTimer >= 50f)
            {
                bool interactBool = Random.value > 0.5f;
                if (interactBool)
                {
                    RandomGhostInteraction();
                }
                interactTimer = 0f;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (type == GhostType.PSYCHOLOGICAL)
        {
              psychologicalType = true;
        }
        if (type == GhostType.BIOLOGICAL)
        {
            biologicalType = true;
        }
        if (type == GhostType.METAPHYSICAL)
        {
            metaphysicalType = true;
        }
        // Finds and gets level manager
        levelManager1 = (LevelManager)FindAnyObjectByType(typeof(LevelManager));
        // Sets the current room ghost is in to spawn room.
        currentRoom = levelManager1.SelectRandomRoom();
        SetGhostPosition(currentRoom.selectRandomSpawnPoint());
        // Sets aggressionThreshold to 1
        aggressionThreshold = 1;
        // Sets aggression to 0.
        aggression = 0;
        // Sets aggression multiplier.
        aggressionMultiplier = 25;
        // Randomizes difficulty level if not specified.
        difficultyLevel = Random.Range(1, 5);
        // Forces difficulty level to be between 1 and 5.
        difficultyLevel = Mathf.Clamp(difficultyLevel, 1, 5);
        // Makes aggression threshold in terms of difficulty. Min 25, Max 125. Lower threshold, easier to aggro ghost and considered "harder".
        for (int i = 5; i>2; i--)
        {
            aggressionThreshold += difficultyLevel*aggressionMultiplier;
        }
        emfLevel = aggressionThreshold / 5;
        // Ensures only one Ghost type is true at a time. ELSE STATEMENTS HERE REQUIRE CLAMP TO WORK
        if (psychologicalType)
        {
            typeName = "Psychological";
        }
        if (biologicalType)
        {
            typeName = "Biological";
        }
        if (metaphysicalType)
        {
            typeName = "Metaphysical";
        }
        // Sets Hunting Zone to all rooms.
        huntingZone = levelManager1.GetAllRooms();
        // Initailzes ghost in passive mode.
        huntingMode = false;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        //Adds Capsule collider for ghost if one doesn't exist.
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        // Tracks current room 
        currentRoom = levelManager1.GetRoomFromPosition(transform.position);
        // Ghost's distance from player.
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        difficultyLevel = Mathf.Clamp(difficultyLevel, 1, 5);
        //Gets collider from Player Body
        Collider col = null;
        foreach (Transform child in player)
        {
            if (child.name == "Player Body")
            {
                col = child.GetComponent<Collider>();
            }
        }
        // When aggression increases enough, hunting mode turns on.
        if (aggression > aggressionThreshold)
        {
            Debug.Log("HUNT");
            huntingMode = true;
        }
        // When ghost is in passive mode, ghost will randomly teleport between rooms.
        if (!huntingMode)
        {
            Roam();
        }

        if (huntingMode)
        {
            huntingTimer += Time.deltaTime;
            // Tracks current room 
            currentRoom = levelManager1.GetRoomFromPosition(transform.position);
            // Makes Ghost visible during hunting mode
            GetComponent<Renderer>().enabled = true;
            //Sets the minimum time a ghost will be hunting you for. Turns off hunting mode after that time.
            if (huntingTimer < 30f)
            {
                //Ghost will move towards player.
                if (distanceFromPlayer > 1)
                {
                    // Every ten seconds in Hunting Mode, changes the Ghost's speed depending on ghost type.
                        if (huntingTimer/10f>=1)
                        {
                            if (psychologicalType)
                            {
                                // Randomly changes speed every 10 seconds
                                moveSpeed = Random.Range(1, 3);
                            }
                             if (biologicalType)
                             {
                            // if difficulty is below lvl3, lowers moveSpeed every 10 seconds
                                if (difficultyLevel < 3 && moveSpeed >= 2)
                                { 
                                    moveSpeed -= 1;
                                }
                             }
                          }
                    
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                huntingTimer = 0f;
                aggression = 0;
                huntingMode = false;
                Debug.Log("hunt end meow");
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!huntingMode)
        {
            aggression++;
        }
        else
        {
            Debug.Log("GAME OVER :C");
        }
    }
    /// <summary>
    /// Returns EMF 
    /// </summary>
    /// <returns>Integer</returns>
    public int GetEmf()
    {
        return emf;
    }
    /// <summary>
    /// Returns type of ghost.
    /// </summary>
    /// <returns>Type name as string</returns>
    public string GetType()
    {
        return typeName;
    }
    /// <summary>
    /// Returns difficulty level as an integer (1-5)
    /// </summary>
    public int GetDifficulty()
    {
        return difficultyLevel;
    }
    /// <summary>
    /// Returns the room the ghost is currently in.
    /// </summary>
    public Room GetGhostRoom()
    {
        return currentRoom;
    }
    /// <summary>
    /// Returns position of ghost
    /// </summary>
    public Vector3 GetGhostLocation()
    {
        return transform.position;
    }

    /// <summary>
    /// Returns 
    /// </summary>
    public bool IsGhostHunting()
    {
        return huntingMode;
    }
    public int GetEmfLevel()
    {
        return emfLevel;
    }
}
