using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The Room component holds references to adjacent rooms and 
/// and ghost interactable objects within the specific room setpiece,
/// organizing these references for the ghost to access and use easily.
/// </summary>
public class Room : MonoBehaviour
{
    /// <summary>
    /// A list of adjacent rooms. Is marked private to encapsulate
    /// direct references. Populating this list should be done via
    /// the "appendAdjacentRoom" method. For static levels without
    /// level generation this list can be populated in the editor.
    /// </summary>
    [SerializeField]
    private List<Room> adjacentRooms;

    /// <summary>
    /// A reference to the component responsible for containing the navmesh
    /// surface.
    /// </summary>
    [SerializeField]
    private NavMeshSurface roomNavmeshSurface;

    /// <summary>
    /// The room's bounding box, representing the bounds of the room in code.
    /// The bounding box is used to determine if the player or player's tools
    /// are in the room.
    /// </summary>
    [SerializeField]
    private Bounds boundingBox;

    /// <summary>
    /// 
    /// 
    /// </summary>
    [SerializeField]
    private List<Transform> spawnPoints;

    /// <summary>
    /// A list of interactables in the room, populated in the 
    /// Start function by searching for Ghost Interactables as children
    /// of the room object.
    /// Refer to this list to access all interactables in the room
    /// </summary>
    [SerializeField]
    public List<GhostInteractable> interactables;

    // Accessors
    public Bounds GetBoundingBox() { return boundingBox; }
    public NavMeshSurface GetNavMeshSurface() { return roomNavmeshSurface; }

    // Awake is called even before Start
    private void Awake()
    {
        // Assign a list of all GhostInteractable objects to this list
        // reference
        interactables = GetComponentsInChildren<GhostInteractable>()
                            .ToList();

        // Set the bounding box
        boundingBox = GetComponent<BoxCollider>().bounds;
    }

    // Functions to be called by the ghost behavior manager

    /// <summary>
    /// Selects a random spawn point from the room's list of ghost spawn points
    /// </summary>
    /// <returns>A Transform object where the ghost can be spawned in the room</returns>
    public Transform selectRandomSpawnPoint()
    {
        int idx = Random.Range(0, spawnPoints.Count);
        return spawnPoints[idx];
    }

    /// <summary>
    /// Use this to draw a random adjacent room from the list of adjacent
    /// rooms.
    /// </summary>
    /// <returns>A randomly selected Room object in this object's 
    /// adjacent room list.</returns>
    public Room selectRandomAdjacentRoom()
    {
        int idx = Random.Range(0, adjacentRooms.Count());
        return adjacentRooms[idx];

    }

    /// <summary>
    /// This method uses generics to filter GhostInteractable objects in the
    /// interactables list by class/type. Specify the type of interest using
    /// generics, and the method will extract objects of that type (as well
    /// as subtypes of the specified type)
    /// </summary>
    /// <typeparam name="T">Essentially the filter criteria</typeparam>
    /// <returns>A List of GhostInteractables containing only
    /// objects matching the filter type.</returns>
    public List<T> filterInteractables<T>() where T : GhostInteractable
    {
        List<T> output = new List<T>();
        foreach (GhostInteractable interactable in interactables)
        {
            if (interactable is T) { 
                output.Append(interactable);
            }
        }

        return output;
    }

    // Level Loading Functions

    /// <summary>
    /// Set adjacency between two rooms a and b, so b will be appended to
    /// the adjacentRooms list of a, and vice versa. This function should really
    /// only be called by the level loader.
    /// </summary>
    /// <param name="a">The first room</param>
    /// <param name="b">The second room</param>
    public static void setRoomAdjacency(Room a, Room b)
    {
        a.adjacentRooms.Append(b);
        b.adjacentRooms.Append(a);
    }
}
