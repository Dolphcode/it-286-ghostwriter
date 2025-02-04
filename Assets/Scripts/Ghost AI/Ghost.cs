using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    // <summary>
    //Amount of times a ghost can be provoked before entering Hunting Mode.
    //</summary>
    public int aggressionThreshold;
    // <summary>
    //Amount of times a ghost has been provoked.
    //</summary>
    public int aggression;
    //<summary>
    //Target the ghost is chasing.
    //</summary>
    public Transform player;
    //<summary>
    // Hunting ghost prefab.
    //</summary>
    public GameObject huntingGhostPrefab;
    //<summary>
    //Room the ghost spawns in.
    //</summary>
    public Room spawnRoom;
    //<summary>
    //Room the ghost is currently in.
    //</summary>
    public Room currentRoom;
    //<summary>
    //NavMesh.
    //</summary>
    private NavMeshAgent agent;
    //<summary>
    //Movement speed of ghost.
    //</summary>
    public float moveSpeed = 5f;
    //<summary>
    //Indication of ghost being in Hunting Mode. 
    //</summary>
    public bool huntingMode;
    //<summary>
    //List of rooms that the ghost can access.
    //</summary>
    public Transform[] huntingZone;
    //<summary>
    //New level manager.
    //</summary>
    LevelManager levelManager1 = new LevelManager();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // Sets spawn room for ghost.
        spawnRoom = levelManager1.SelectRandomRoom();
        // Sets the current room ghost is in to spawn room.
        currentRoom = spawnRoom;
        aggressionThreshold = Random.Range(2, 5);
        aggression = 0;
        huntingMode = false;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        //Adds Capsule collider for ghost if one doesn't exist.
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
        }
        //Sets hunting zone for the ghost (use adjacent rooms)

    }
    // Update is called once per frame
    public void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        // When player provokes ghost, aggression counter increases.
        if //player provokes ghost
        {
            aggression++;
        }
        // When player provokes ghost, aggression counter increases.
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
            if (distanceFromPlayer > 1)
            {
                //<summary>
                //Ghost will move towards player.
                //</summary>
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                //check if collided with player here
            }
        }
        //<summary>
        //Ghost will switch locations rooms and randomly depending on aggression level.
        //</summary>
        void Roam()
        {
            float interactTimer = 0f;
            float teleportTimer = 0f;
            if (aggression > aggressionThreshold / 2)
            {
                teleportTimer += Time.deltaTime;
                interactTimer += Time.deltaTime;
                if (teleportTimer >= 120f)
                {
                    currentRoom = levelManager1.SelectRandomRoom();
                }
                bool interactBool = Random.value > 0.75f;
                if (interactTimer >= 20)
                {
                    //object will implement GhostInteractable interface; doesn't exist yet
                    object.interact();
                }
            }
            else
            {
                teleportTimer += Time.deltaTime;
                interactTimer += Time.deltaTime;
                if (teleportTimer >= 180f)
                {
                    currentRoom = levelManager1.SelectRandomRoom();
                }
                if (interactTimer >= 50f)
                {
                    bool interactBool = Random.value > 0.5f;
                    if (interactBool)
                    {
                        //object will implement GhostInteractable interface; doesn't exist yet
                        object.interact();
                    }
                }
            }
        }
    }
}
