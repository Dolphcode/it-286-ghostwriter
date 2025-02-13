using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    // <summary>
    //Amount of times a ghost can be provoked before entering Hunting Mode.
    //</summary>
    [SerializeField]
    private int aggressionThreshold;
    // <summary>
    //Amount of times a ghost has been provoked.
    //</summary>
    [SerializeField]
    private int aggression;
    // <summary>
    // Aggression multiplier of ghost. Lowers difficulty based on multiplier.
    //</summary>
    [SerializeField]
    private int aggressionMultiplier;
    // <summary>
    // Difficulty of ghost (lower level = higher aggression threshold, slower)
    //</summary>
    [SerializeField]
    private int difficultyLevel;
    // <summary>
    // Ghost type Psychological (more erratic behavior, speed changes, many interactions).
    // EMF changes from 1,5;
    // </summary>
    [SerializeField]
    private bool psychologicalType;
    // <summary>
    // Ghost type Biological (died a natural death - not as fast, hard to aggro).
    // EMF: 1
    // </summary>
    [SerializeField]
    private bool biologicalType;
    // <summary>
    // Ghost type Metaphysical (died but spiritually - aggression threshold lowers the more you aggro them. has a lot more interactions/tries to communicate with player more? maybe triggers a certain tool).
    // EMF: 5
    // </summary>
    [SerializeField]
    private bool metaphysicalType;
    // <summary>
    // String containting name of ghost type.
    // </summary>
    [SerializeField]
    private string typeName;
    // <summary>
    // EMF variable.
    // </summary>
    [SerializeField]
    private int EMF;
    //<summary>
    //Target the ghost is chasing.
    //</summary>
    [SerializeField]
    private Transform player;
    //<summary>
    // Hunting ghost prefab.
    //</summary>
    [SerializeField]
    private GameObject huntingGhostPrefab;
    //<summary>
    //Room the ghost is currently in. Initialized during start to be random room.
    //</summary>
    [SerializeField]
    private Room currentRoom;
    //<summary>
    //NavMesh.
    //</summary>
    [SerializeField]
    private NavMeshAgent agent;
    //<summary>
    //Movement speed of ghost.
    //</summary>
    [SerializeField]
    private float moveSpeed = 1f;
    //<summary>
    //Indication of ghost being in Hunting Mode. 
    //</summary>
    [SerializeField]
    private bool huntingMode;
    //<summary>
    //List of rooms that the ghost can access.
    //</summary>
    [SerializeField]
    private System.Collections.Generic.List<Room> huntingZone;
    //<summary>
    //Get level manager.
    //</summary>
    [SerializeField]
    private LevelManager levelManager1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        levelManager1 = (LevelManager)FindAnyObjectByType(typeof(LevelManager));
        // Sets the current room ghost is in to spawn room.
        currentRoom = levelManager1.SelectRandomRoom();
        setGhostPosition(currentRoom.selectRandomSpawnPoint());
        // Sets random aggressionThreshold to 1
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
        for (int i = 5; i>difficultyLevel; i--)
        {
            aggressionThreshold *= aggressionMultiplier;
        }
        // Ensures only one Ghost type is true at a time. ELSE STATEMENTS HERE REQUIRE CLAMP TO WORK
        if (psychologicalType)
        {
            biologicalType = false;
            metaphysicalType = false;
            // More difficult level means more range of EMF.
            if (difficultyLevel >= 3)
            {
                EMF = Random.Range(1, 5);
            }
            else
            {
                EMF = Random.Range(2, 3);
            }
            typeName = "Psychological";
        }
        if (biologicalType)
        {
            psychologicalType = false;
            metaphysicalType = false;
            // Easier difficulty means less range of EMF.
            if (difficultyLevel < 3)
            {
                EMF = 1;
            }
            else
            {
                EMF = Random.Range(1, 2);
            }
            typeName = "Biological";
        }
        if (metaphysicalType)
        {
            biologicalType = false;
            metaphysicalType = false;
            // Easier difficulty means less range of EMF.
            if (difficultyLevel < 3)
            {
                EMF = 5;
            }
            else
            {
                EMF = Random.Range(1, difficultyLevel);
            }
            EMF = Random.Range(3, 5);
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
    //<summary>
    //Sets ghost position to room.
    //</summary>
    private void setGhostPosition(Transform spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
    }
    // Update is called once per frame
    private void Update()
    {
        // Ghost's distance from player.
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        // ALSO IN START, HERE FOR TESTING IN SCENE: Ensures only one Ghost type is true at a time.
        if (psychologicalType)
        {
            biologicalType = false;
            metaphysicalType = false;
        }
        if (biologicalType)
        {
            psychologicalType = false;
            metaphysicalType = false;
        }
        if (metaphysicalType)
        {
            biologicalType = false;
            metaphysicalType = false;
        }
        difficultyLevel = Mathf.Clamp(difficultyLevel, 1, 5);
        //END OF STUFF HERE JUST FOR TESTING
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
            huntingMode = true;
        }
        //<summary>
        //When ghost is in passive mode, ghost will randomly teleport between rooms.
        //</summary>
        if (!huntingMode)
        {
            Roam();
        }
        if (huntingMode)
        {
            float huntingTimer = 0f;

            //Sets the minimum time a ghost will be hunting you for. Turns off hunting mode after that time.
            if (huntingTimer < 30f)
            {
                huntingTimer += Time.deltaTime;

                //Ghost will move towards player.
                if (distanceFromPlayer > 1)
                {
                    // Every ten seconds in Hunting Mode, changes the Ghost's speed depending on ghost type.
                        if (huntingTimer <10f)
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
                aggression = 0;
                huntingMode = false;
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
    //<summary>
    //Ghost will switch locations rooms and randomly depending on aggression level.
    //</summary>
    private void Roam()
        {
            float interactTimer = 0f;
            float teleportTimer = 0f;
            float emfTimer = 0f;
        // If aggression more than half full.
              if (aggression > aggressionThreshold / 2)
              {
                teleportTimer += Time.deltaTime;
                interactTimer += Time.deltaTime;
                if (teleportTimer >= 120f)
                {
                    currentRoom = levelManager1.SelectRandomRoom();
                    setGhostPosition(currentRoom.selectRandomSpawnPoint());
            }
                bool interactBool = Random.value > 0.75f;
                if (interactTimer >= 20)
                {
                    randomGhostInteraction();
                }
                // The harder is it, the more often EMF variable will change
                if (emfTimer >=100*1/difficultyLevel && psychologicalType)
                {
                    emfTimer += Time.deltaTime;
                    EMF = Random.Range(2, 3);
                }
              }
              else
              {
                teleportTimer += Time.deltaTime;
                interactTimer += Time.deltaTime;
                if (teleportTimer >= 180f)
                {
                    currentRoom = levelManager1.SelectRandomRoom();
                    setGhostPosition(currentRoom.selectRandomSpawnPoint());
            }
                if (interactTimer >= 50f)
                {
                    bool interactBool = Random.value > 0.5f;
                    if (interactBool)
                    {
                    randomGhostInteraction();
                    }
                }
              }
        }
    //<summary>
    //Will randomize which interaction happens.
    //</summary>
    private void randomGhostInteraction()
        {
            // Sets variable randInteract to random number between 0 and max number of interactables. Can just set to however many interacts are implemented in the future.
            int maxInteract = 1;
            int randInteract = Random.Range(0, maxInteract);
            //delete after; just to test to make sure interact works
            randInteract = 0; 
            if (randInteract==0)
            { 
                currentRoom.filterInteractables<RoomLightInteractable>();
            //add smth for each ghost type
                //other interactables
                // currentRoom.filterInteractables<> returns list of interactables, if iteratables are true for ghost type then randomly pick
            }
        }   
    // <summary>
    // Returns EMF
    // </summary>
    private int getEMF()
    {
        return EMF;
    }
    // <summary>
    // Returns type name as a string
    // </summary>
    private string getType()
    {
        return typeName;
    }
    // <summary>
    // Returns difficulty level as an integer (1-5)
    // </summary>
    private int getDifficulty()
    {
        return difficultyLevel;
    }
    // <summary>
    // Returns the room the ghost is currently in
    // </summary>
    private Room getGhostRoom()
    {
        return currentRoom;
    }
    // <summary>
    // Returns position of ghost
    // </summary>
    private Vector3 getGhostLocation()
    {
        return transform.position;
    }
}
