using System.Collections.Generic;
using System.Linq;
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
    /// A list of interactables in the room, populated in the 
    /// Start function by searching for Ghost Interactables as children
    /// of the room object.
    /// Refer to this list to access all interactables in the room
    /// </summary>
    public List<GhostInteractable> interactables { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign a list of all GhostInteractable objects to this list
        // reference
        interactables = GetComponentsInChildren<GhostInteractable>()
                            .ToList<GhostInteractable>();
    }

    // Functions to be called by the ghost behavior manager

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
