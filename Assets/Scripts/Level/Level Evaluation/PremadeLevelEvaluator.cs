using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class representing the level evaluator for a pre-designed level (Requiring
/// no room generation/procedural generation whatsoever). In this case, all we
/// need to do is build the navmesh.
/// </summary>
[CreateAssetMenu(fileName = "Unnamed Level Evaluator", menuName = "Scriptable Objects/Premade Level Evaluator")]
public class PremadeLevelEvaluator : LevelEvaluator
{
    public override List<Room>[] InitializeInterior(GameObject g, int zones)
    {
        rooms = new List<Room>();
        foreach (Room room in g.GetComponentsInChildren<Room>())
        {
            // Initialize the room list from the list of rooms in the interior
            // environment
            rooms.Add(room);

            // Build the navmesh
            room.GetNavMeshSurface().BuildNavMesh();
        }

        // TODO: List<Room>[] zones = new List<Room>[zones];
        List<Room>[] zoneArr = new List<Room>[1];
        zoneArr[0] = rooms;
        return zoneArr;
    }
}
