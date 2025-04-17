using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

///<summary>
/// Ghost Types dropdown.
/// Psychological (more erratic behavior, speed changes, many interactions). 
/// Biological (died a natural death - not as fast, hard to aggro). 
/// Metaphysical (died but spiritually - aggression threshold lowers the more you aggro them. 
/// Has a lot more interactions/tries to communicate with player more? maybe triggers a certain tool).
/// </summary>
public enum GhostType
{
    PSYCHOLOGICAL, BIOLOGICAL, METAPHYSICAL
}

public class Ghost : Capturable
{
    private Dictionary<GhostType, GhostTypeData> ghostTypeMap = new();
    public static List<GameObject> ghostModelsList = new List<GameObject>();
    public static List<GhostInteractable> ghostTypeInteractables = new List<GhostInteractable>();
    private GhostTypeData ghostData;
    ///<summary>
    ///Target the ghost is chasing.
    ///</summary>
    [SerializeField]
    private Transform player;
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
    /// <summary>
    ///Reference to the female ghost model.
    ///</summary>
    [SerializeField]
    private GameObject ghostModel;
    /// <summary>
    ///Amount of times a ghost can be provoked before entering Hunting Mode.
    ///</summary>
    [SerializeField]
    private int aggressionThreshold;
    /// <summary>
    ///Amount of times a ghost can be provoked before entering Hunting Mode.
    ///</summary>
    [SerializeField]
    int thresholdChange;
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
    private int difficultyLevel = 0;
    /// <summary>
    /// Ghost's types
    /// </summary>
    [SerializeField]
    private GhostType type;
    ///<summary>
    ///String containting name of ghost type.
    ///</summary>
    private string typeName;
    ///<summary>
    ///EMF variable.
    ///</summary>
    [SerializeField]
    private int emf = 1;
    ///<summary>
    /// EMF Player has to track (highest EMF)
    ///</summary>
    private int maxEMF;
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
    ///Indication of ghost being in Hunting Mode. 
    ///</summary>
    [SerializeField]
    private bool huntingMode;
    ///<summary>
    ///List of rooms that the ghost can access.
    ///</summary>
    [SerializeField]
    private List<Room> huntingZone;
    ///<summary>
    ///Level manager.
    ///</summary>
    [SerializeField]
    public LevelManager levelManager1;
    private float aggroTimer = 0f;
    private float interactTimer = 0f;
    private float teleportTimer = 0f;
    private float huntingTimer = 0f;
    private float functionTimer = 0f;
    private Animator modelAnimController;
    ///<summary>
    /// Returns random room in Hunting Zone
    ///</summary>
    public Room SelectRandomHuntingRoom()
    {
        int idx = Random.Range(0, huntingZone.Count);
        return huntingZone[idx];
    }
    ///<summary>
    /// Sets ghost position to room.
    ///</summary>
    private void SetGhostPosition(Transform spawnPoint)
    {
        //transform.position = spawnPoint.transform.position;
        agent.Warp(spawnPoint.transform.position);
        Physics.SyncTransforms();
        Debug.Log("GHOST POSITION CHANGE");
    }
    ///<summary>
    /// Will randomize which interaction happens.
    ///</summary>
    private void RandomGhostInteraction()
    {
        Debug.Log("OBJECT INTERACT");
        //random chance of interact happening
        int randInteract = Random.Range(0, currentRoom.FilterInteractables(GhostInteractableType.Lights, GhostInteractableType.Fingerprint, GhostInteractableType.Movable).Count);
        currentRoom.FilterInteractables(GhostInteractableType.Lights, GhostInteractableType.Fingerprint, GhostInteractableType.Movable)[randInteract].interact();

    }
    ///<summary>
    /// Ghost will teleport to an adjacent room once every given input time (float) if room is within hunting zone
    /// Requires that one of the adjacent rooms is in the hunting zone or it will end up as a recursive hellloop
    ///</summary>
    private void GhostTeleportsAdjacentRoom(float time)
    {
        Room possibleRoom = currentRoom.SelectRandomAdjacentRoom();
        bool validRoom = false;
        // Checks if room is in hunting zone
        foreach (Room room in huntingZone)
        {
            //Debug.Log("At time " + teleportTimer.ToString() + " Room 1 " + room.name + " Room 2 " + possibleRoom.name + " what ? " + (room == possibleRoom).ToString());
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
        /*
        if (!validRoom)
        {
            GhostTeleportsAdjacentRoom(time);
        }*/
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
        teleportTimer += Time.deltaTime;
        interactTimer += Time.deltaTime;
        aggroTimer += Time.deltaTime;
        if (aggression >= thresholdChange && emf < maxEMF)
        {
            emf++;
            thresholdChange += aggressionThreshold / maxEMF;
        }
        if (levelManager1.IsPlayerInRoom(currentRoom))
        {
            if (aggroTimer >= 1f)
            {
                aggression += 2;
                aggroTimer = 0f;
            }
        }
        // Ghost is not visible when in passive.
        GetComponent<Renderer>().enabled = false;
        // Disables model
        ghostModel.SetActive(false);
        // If aggression less than half full game is slightly harder
        if (aggression < aggressionThreshold / 2)
        {
            GhostTeleportsAdjacentRoom(80f);
            //takes longer, less chance = harder
            GhostInteracts(15f, 0.25);
        }
        // When ghost is in second half of aggression threshold
        else if (aggression < aggressionThreshold)
        {
            GhostTeleportsAdjacentRoom(60f);
            GhostInteracts(10f, 0.5);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ///<summary>
        /// Contains each GhostType's  typeName, moveSpeed, aggressionMultiplier, maxEMF
        ///</summary>
        Dictionary<GhostType, GhostTypeData> ghostTypeMap = new()
        {
            { GhostType.PSYCHOLOGICAL, new GhostTypeData("Psychological", Random.Range(1f,2f), Random.Range(15,31), 5, ghostModelsList, ghostTypeInteractables)},
            { GhostType.BIOLOGICAL, new GhostTypeData("Biological", 1f, 20, 2, ghostModelsList, ghostTypeInteractables)},
            { GhostType.METAPHYSICAL, new GhostTypeData("Metaphysical", 1.5f, 25, 4, ghostModelsList, ghostTypeInteractables)}
        };
        // Disables model
        ghostModel.SetActive(false);
        // care the Female_Ghost vs Female Ghost (same w male)
        modelAnimController = ghostModel.GetComponent<Animator>();
        ghostModel = transform.Find("SEb_Ghost")?.gameObject;
        // Sets aggressionThreshold to 1
        aggressionThreshold = 1;
        // Sets aggression to 0.
        aggression = 0;
        // Sets aggression multiplier.
        aggressionMultiplier = ghostData.aggressionMultiplier;
        difficultyLevel = Random.Range(1, 6);
        // Makes aggression threshold in terms of difficulty. Min 25, Max 125. Lower threshold, easier to aggro ghost and considered "harder".
        for (int i = 5; i>2; i--)
        {
            aggressionThreshold += Mathf.RoundToInt(difficultyLevel*0.5f*aggressionMultiplier);
        }
        maxEMF = ghostData.maxEMF;
        thresholdChange = aggressionThreshold / maxEMF;
        if (type == GhostType.PSYCHOLOGICAL)
        {
            ghostData = ghostTypeMap[GhostType.PSYCHOLOGICAL];
        }
        if (type == GhostType.BIOLOGICAL)
        {
            ghostData = ghostTypeMap[GhostType.BIOLOGICAL];
        }
        if (type == GhostType.METAPHYSICAL)
        {
            ghostData = ghostTypeMap[GhostType.METAPHYSICAL];
        }
        // Initailzes ghost in passive mode.
        huntingMode = false;
        agent.speed = ghostData.moveSpeed;
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
        // DO NOT DO ANYTHING until level is started
        if (levelManager1 == null || !levelManager1.levelStarted) return;
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
            thresholdChange = aggressionThreshold / maxEMF;
            modelAnimController.SetFloat("MoveSpeed", -1f);
            /* uncomment this if you want to modulate aggro based on aggression threshold
            modelAnimController.SetFloat("Aggro",
                Mathf.Clamp(aggression / aggressionThreshold, 0, 1) * 2f - 1f);*/
            modelAnimController.SetFloat("Aggro", -1f);
            agent.SetDestination(transform.position); // A little hacky but it gets the job done
        }
        if (huntingMode)
        {
            m_TriggerCapture.Invoke(this);

            // Set the animation controller blending
            // Both male and female model controllers have the same parameters
            modelAnimController.SetFloat("MoveSpeed", 0f);
            modelAnimController.SetFloat("Aggro", -1f);

            huntingTimer += Time.deltaTime;
            // Tracks current room 
            currentRoom = levelManager1.GetRoomFromPosition(transform.position);
            ghostModel.SetActive(true);
            //Sets the minimum time a ghost will be hunting you for. Turns off hunting mode after that time.
            bool validRoom = false;
            if (huntingTimer < 30f)
            {
                //Ghost will move towards player.
                if (distanceFromPlayer > 1)
                {
                    foreach (Room room in huntingZone)
                    {
                        if (room == currentRoom)
                        {
                            validRoom = true;
                        }
                    }
                    if (validRoom)
                    {
                        // Ghost moves towards player
                        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                        Debug.Log("Is on navmesh: " + agent.isOnNavMesh.ToString());
                        Debug.Log("Is active and enabled: " + agent.isActiveAndEnabled.ToString());
                        //if (Vector3.Distance(transform.position, player.position) > currentRoom.GetRoomBoundsets().size.x)
                            agent.speed = ghostData.moveSpeed;
                        //else
                        {
                            agent.speed = ghostData.moveSpeed;
                        }
                        agent.SetDestination(player.transform.position);
                        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                    }
                    // If the room the ghost is in is not a valid room, ghost will spawn into a random point within hunting zone and end of hunting mode occurs
                    else
                    {
                        SetGhostPosition(SelectRandomHuntingRoom().SelectRandomSpawnPoint());
                        huntingTimer = 0f;
                        aggression = 0;
                        huntingMode = false;
                        Debug.Log("Hunt End");
                    }
                }
            }
            else
            {
                huntingTimer = 0f;
                aggression = 0;
                huntingMode = false;
                Debug.Log("Hunt End");
            }
        }
    }
    /// <summary>
    /// If player is in ghost, aggression increases.
    /// </summary>
    private void OnTriggerEnter(Collider col)
    {
        if (!huntingMode)
        {
            aggression += 2;
        }
        else
        {
            Debug.Log("GAME OVER");
            levelManager1.LoseLevel();
        }
    }
    // Getter Functions
    /// <summary>
    /// Returns EMF 
    /// </summary>
    /// <returns>Integer</returns>
    public int GetEmf()
    {
        return emf;
    }
    /// <summary>
    /// Returns tracked EMF (for ghost type).
    /// </summary>
    /// <returns>Integer</returns>
    public int GetMaxEmf()
    {
        return maxEMF;
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
    /// Returns difficulty level as an integer (1-5)
    /// </summary>
    public int GetDifficulty()
    {
        return difficultyLevel;
    }
    /// <summary>
    /// Returns aggresion threshold.
    /// </summary>
    public int GetAggressionThreshold()
    {
        return aggressionThreshold;
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
    // Setter Functions

    /// <summary>
    /// Sets enum type of ghost.
    /// </summary>
    public void SetGhostType(GhostType ghostType)
    {
        type = ghostType;
    }
    public void SetBodyType(bool boo)
    {

    }
    /// <summary>
    /// Sets enum type of ghost based on integer.
    /// </summary>
    public void SetGhostType(int randNum)
    {
        if (randNum == 1)
        { type = GhostType.PSYCHOLOGICAL; }
        else if (randNum == 2)
        { type = GhostType.BIOLOGICAL; }
        else if (randNum == 3)
        { type = GhostType.METAPHYSICAL; }
    }
    /// <summary>
    /// Sets enum type of ghost based on string.
    /// </summary>
    public void SetGhostType(string a)
    {
        if (a == "P" || a == "PSYCH")
        { type = GhostType.PSYCHOLOGICAL; }
        else if (a == "B" || a == "BIO")
        { type = GhostType.BIOLOGICAL; }
        else if (a == "M" || a == "META" || a == "PHYS")
        { type = GhostType.METAPHYSICAL; }
    }
    /// <summary>
    /// Sets difficulty level as an integer (forced to be between 1-5).
    /// </summary>
    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 5);
    }
    /// <summary>
    /// Sets aggresion threshold based on input number.
    /// </summary>
    public void SetAggressionThreshold(int newThreshold)
    {
        aggressionThreshold = newThreshold;
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
        currentRoom = huntingZone[Random.Range(0, huntingZone.Count)];

        SetGhostPosition(currentRoom.SelectRandomSpawnPoint());
    }

    // Functions for Tools

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
        SetGhostPosition(SelectRandomHuntingRoom().SelectRandomSpawnPoint());
        huntingMode = false;
        huntingTimer = 0f;
        aggression = 0;
    }
    /// <summary>
    /// Increases aggresssion by 1
    /// </summary>
    public void IncreaseAggression()
    {
        aggression += 2;
    }
    /// <summary>
    /// Increases aggresssion by integer input
    /// </summary>
    public void IncreaseAggression(int increase)
    {
        aggression += increase;
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
    /// Lowers aggresion threshold based on input number.
    /// </summary>
    public void LowerAggressionThreshold(int lowerBy)
    {
        aggressionThreshold -= lowerBy;
    }
    public override GameObject GetCheckObject()
    {
        return gameObject;
    }
    public override int GetCaptureScore(float rayProp, CaptureData data)
    {
        return (IsGhostHunting() ? 5 : 2);
    }
    public void Awake()
    {
        m_TriggerCapture = new UnityEvent<Capturable>();
    }
}