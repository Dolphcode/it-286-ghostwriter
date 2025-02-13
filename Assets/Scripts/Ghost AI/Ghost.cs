using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    //Room the ghost is currently in. Initialized during start to be random room.
    //</summary>
    public Room currentRoom;
    //<summary>
    //NavMesh.
    //</summary>
    private NavMeshAgent agent;
    //<summary>
    //Movement speed of ghost.
    //</summary>
    public float moveSpeed = 1f;
    //<summary>
    //Indication of ghost being in Hunting Mode. 
    //</summary>
    public bool huntingMode;
    //<summary>
    //List of rooms that the ghost can access.
    //</summary>
    public System.Collections.Generic.List<Room> huntingZone;
    //<summary>
    //Get level manager.
    //</summary>
    LevelManager levelManager1 = (LevelManager)FindAnyObjectByType(typeof(LevelManager));
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // Sets the current room ghost is in to spawn room.
        currentRoom = levelManager1.SelectRandomRoom();
        setGhostPosition(currentRoom);
        // Sets random aggressionThreshold
        aggressionThreshold = Random.Range(2, 5);
        // Sets aggression to 0.
        aggression = 0;
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
    public void setGhostPosition(Room room)
    {
        transform.position = room.transform.position;
    }
    // Update is called once per frame
    public void Update()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
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
                if (distanceFromPlayer > 1)
                {
                    //Ghost will move towards player.
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
            }
            else
            {
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
                    setGhostPosition(currentRoom);
            }
                bool interactBool = Random.value > 0.75f;
                if (interactTimer >= 20)
                {
                    //object will implement GhostInteractable interface; doesn't exist yet
                    //object.interact();
                }
            }
            else
            {
                teleportTimer += Time.deltaTime;
                interactTimer += Time.deltaTime;
                if (teleportTimer >= 180f)
                {
                    currentRoom = levelManager1.SelectRandomRoom();
                    setGhostPosition(currentRoom);
                }
                if (interactTimer >= 50f)
                {
                    bool interactBool = Random.value > 0.5f;
                    if (interactBool)
                    {
                        //object will implement GhostInteractable interface; doesn't exist yet
                        //object.interact();
                    }
                }
            }
        }
    }
