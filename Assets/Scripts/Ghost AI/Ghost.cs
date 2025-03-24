using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
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
public class GhostAI : MonoBehaviour
{
    ///<summary>
    ///Target the ghost is chasing.
    ///</summary>
    [SerializeField]
    private Transform player;
    public void SetPlayer(Transform player) 
    { 
        this.player = player; 
    }
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
    /// <summary>
    /// Determines model type. true - fem, masc - false.
    ///</summary>
    [SerializeField]
    private bool bodyTypeF;
    ///<summary>
    ///Difficulty of ghost (lower level = higher aggression threshold, slower)
    ///</summary>
    [SerializeField]
    private int difficultyLevel = 0;
    /// <summary>
    /// Ghost's type.
    /// </summary>
    [SerializeField]
    private GhostType type;
    ///<summary>
    ///Ghost type Psychological (more erratic behavior, speed changes, many interactions).
    ///</summary>
    private bool psychologicalType;
    ///<summary>
    ///Ghost type Biological (died a natural death - not as fast, hard to aggro).
    ///</summary>
    private bool biologicalType;
    ///<summary>
    ///Ghost type Metaphysical (died but spiritually - aggression threshold lowers the more you aggro them. has a lot more interactions/tries to communicate with player more? maybe triggers a certain tool).
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
    private float moveSpeed;
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
    ///Number of rooms that the ghost can access.
    ///</summary>
    [SerializeField]
    private int huntingZoneRoomCount;
    ///<summary>
    ///Level manager.
    ///</summary>
    [SerializeField]
    private LevelManager levelManager1;
    private float aggroTimer = 0f;
    private float interactTimer = 0f;
    private float teleportTimer = 0f;
    private float huntingTimer = 0f;
    private float functionTimer = 0f;

    ///<summary>
    ///Returns random room in Hunting Zone
    ///</summary>
    public Room SelectRandomHuntingRoom()
    {
        int idx = Random.Range(0, huntingZone.Count);
        return huntingZone[idx];
    }
    ///<summary>
    ///Sets ghost position to room.
    ///</summary>
    private void SetGhostPosition(Transform spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        Physics.SyncTransforms();
        Debug.Log("GHOST POSITION CHANGE");
    }
    ///<summary>
    ///Will randomize which interaction happens.
    ///</summary>
    private void RandomGhostInteraction()
    {
        Debug.Log("OBJECT INTERACT");
        //random chance of interact happening
        bool doesInteract = Random.Range(0, 2) == 0;
        int randInteract = Random.Range(0, currentRoom.FilterInteractables(GhostInteractableType.Fingerprint, GhostInteractableType.Movable).Count);
        if (doesInteract)
        {
            currentRoom.FilterInteractables(GhostInteractableType.Fingerprint, GhostInteractableType.Movable)[randInteract].interact();
        }
    }
    ///<summary>
    ///Ghost will teleport to an adjacent rooom once every given input time (float) if room is within hunting zone
    ///Requires that one of the adjacent rooms is in the hunting zone or it will end up as a recursive hellloop
    ///</summary>
    private void GhostTeleportsAdjacentRoom(float time)
    {
        Room possibleRoom = currentRoom.SelectRandomAdjacentRoom();
        bool validRoom = false;
        // Checks if room is in hunting zone
        foreach (Room room in huntingZone)
        {
            if (room == possibleRoom)
            {
                validRoom = true;
                if (teleportTimer >= time)
                {
                    currentRoom = possibleRoom;
                    SetGhostPosition(currentRoom.SelectRandomSpawnPoint());
                    teleportTimer = 0f;
                }
            }
        }
        if (!validRoom)
        {
            GhostTeleportsAdjacentRoom(time);
        }
    }
    ///<summary>
    /// Ghost will have a chance (double) to interact in a room every given input time (float)
    ///</summary>
    private void GhostInteracts(float time, double chance)
    {
        if (interactTimer >= time)
        {
            double likeliness = 1.0 - chance;
            bool interactBool = Random.value > likeliness;
            if (interactBool)
            {
                RandomGhostInteraction();
            }
            interactTimer = 0f;
        }
    }
    ///<summary>
    ///Ghost will switch locations rooms and randomly depending on aggression level.
    ///</summary>
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
        GetComponent<Renderer>().enabled = false;
        // Gets model or first child
        GameObject child = transform.GetChild(0).gameObject;
        // Disables model
        child.GetComponent<Renderer>().enabled = false;
        // If aggression less than half full game is slightly harder
        if (aggression < aggressionThreshold / 2)
        {
            GhostTeleportsAdjacentRoom(90f);
            //takes longer, less chance = harder
            GhostInteracts(180f, 0.25);
        }
        // When ghost is in second half of aggression threshold
        else if (aggression < aggressionThreshold)
        {
            GhostTeleportsAdjacentRoom(50f);
            GhostInteracts(100f, 0.5);
        }
    }
    /// <summary>
    /// Assigns a random hunting zone with given # of rooms
    /// ***NOTE TO SELF: this code is kinda stupid but also maybe check if selectRandomAdjacentRoom() isn't og room...
    /// </summary>
    private void RandomHuntingZone(Room spawnRoom, int numOfRooms)
    {
        //Adds spawn room as a valid room in huntingZone
        huntingZone.Add(spawnRoom);
        Debug.Log("added ghost");
        numOfRooms--;
        Room a = spawnRoom.SelectRandomAdjacentRoom();
        Room b = spawnRoom.SelectRandomAdjacentRoom().SelectRandomAdjacentRoom();
        for (int i = 0; i < numOfRooms; i++)
        {
            while (a == b)
            {
                a = spawnRoom.SelectRandomAdjacentRoom();
                b = spawnRoom.SelectRandomAdjacentRoom().SelectRandomAdjacentRoom();
            }
            Room c = spawnRoom.SelectRandomAdjacentRoom().SelectRandomAdjacentRoom().SelectRandomAdjacentRoom();
            // Makes sure rooms aren't added twice (each room is unique)
            foreach (Room room in huntingZone)
            {
                if (room != a)
                {
                    huntingZone.Add(a);
                }
                else if (room != b)
                {
                    huntingZone.Add(b);
                }
                else if (room != c)
                {
                    huntingZone.Add(c);
                }
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
        RandomHuntingZone(currentRoom, huntingZoneRoomCount);
        SetGhostPosition(currentRoom.SelectRandomSpawnPoint());
        // Sets aggressionThreshold to 1
        aggressionThreshold = 1;
        // Sets aggression to 0.
        aggression = 0;
        // Sets aggression multiplier.
        aggressionMultiplier = 25;
        // Default move speed for Level 1 Ghost is 1f; increases by 0.25f for each increase in level.
        moveSpeed = 1f;
        if (difficultyLevel>1)
        {
            moveSpeed += difficultyLevel*0.25f;
        }
        // Makes aggression threshold in terms of difficulty. Min 25, Max 125. Lower threshold, easier to aggro ghost and considered "harder".
        for (int i = 5; i>2; i--)
        {
            aggressionThreshold += difficultyLevel*aggressionMultiplier;
        }
        emfLevel = aggressionThreshold / 5;
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
        // Adds Capsule collider for ghost if one doesn't exist.
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
        }
        // Set player Transform if null
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        // Gets collider from Player Body
        Collider col = null;
        foreach (Transform child in player)
        {
            if (child.name == "Player Body")
            {
                col = child.GetComponent<Collider>();
            }
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
            // Gets model or first child
            GameObject child = transform.GetChild(0).gameObject;
            // Enables model
            child.GetComponent<Renderer>().enabled = true;
            //Sets the minimum time a ghost will be hunting you for. Turns off hunting mode after that time.
            bool validRoom = false;
            if (huntingTimer < 30f)
            {
                //Ghost will move towards player.
                if (distanceFromPlayer > 1)
                {
                    foreach (Room room in huntingZone)
                    {
                        if (room==currentRoom)
                        {
                            validRoom = true;
                        }
                    }
                    if (validRoom)
                    {
                        // Every ten seconds in Hunting Mode, changes the Ghost's speed depending on ghost type.
                        if (huntingTimer >= 10f)
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
                        // Ghost moves towards player
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                    }
                    // If the room the ghost is in is not a valid room, ghost will spawn into a random point within hunting zone and end of hunting mode occurs
                    else
                    {
                        SetGhostPosition(SelectRandomHuntingRoom().SelectRandomSpawnPoint());
                        huntingTimer = 0f;
                        aggression = 0;
                        huntingMode = false;
                        Debug.Log("hunt end meow");
                    }
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
    public string GetGhostType()
    {
        return typeName;
    }
    /// <summary>
    /// Sets enum type of ghost.
    /// </summary>
    public void SetGhostType(GhostType ghostType)
    {
        this.type = ghostType;
    }
    /// <summary>
    /// Returns difficulty level as an integer (1-5)
    /// </summary>
    public int GetDifficulty()
    {
        return difficultyLevel;
    }
    /// <summary>
    /// Sets difficulty level as an integer (forced to be between 1-5).
    /// </summary>
    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 5);
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
    /// Returns boolean for if ghost is in hunting mode or not
    /// </summary>
    public bool IsGhostHunting()
    {
        return huntingMode;
    }

    /// <summary>
    /// Turns Ghost Hunting Mode off
    /// </summary>
    public void GhostHuntOff()
    {
        huntingMode = false;
    }
    /// <summary>
    /// Increases aggresssion by 1
    /// </summary>
    public void IncreaseAggression()
    {
        aggression++;
    }
    /// <summary>
    /// Increases aggresssion by integer input
    /// </summary>
    public void IncreaseAggression(int increase)
    {
        aggression=aggression+ increase;
    }
    /// <summary>
    /// Increases aggresssion by integer input, by a factor of the second argument (time in seconds)
    /// Ex: IncreaseAggression(3, 2, 10) increases aggression by 3 every 2 seconds for 10 seconds
    /// </summary>
    public void IncreaseAggression(int increase, int timeOften, int timeEnd)
    {
        functionTimer = 0f;
        while (functionTimer<timeEnd)
        {
            aggression += increase;
            new WaitForSeconds(timeOften);
            functionTimer+=timeOften;
        }
    }
    /// <summary>
    /// Returns EMF level
    /// </summary>
    public int GetEmfLevel()
    {
        return emfLevel;
    }
    /// <summary>
    /// Sets body type/model for ghost. true - fem, masc - false
    /// </summary>
    public void SetBodyTypeF(bool body)
    {
        bodyTypeF = body;
    }
    /// <summary>
    /// Sets body type/model for ghost. true - fem, masc - false
    /// </summary>
    public void SetHuntingZone(System.Collections.Generic.List<Room> huntingArea)
    {
        foreach (Room room in huntingArea)
        {
            huntingZone.Add(room);
        }
    }
}
