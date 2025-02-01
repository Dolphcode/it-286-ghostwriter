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
    public override void InitializeInterior(GameObject g)
    {
        if (initialized)
        {
            Debug.LogWarning("Attempted to initialize interior, but interior has already been initialized");
            return;
        }

        foreach (Room room in g.GetComponentsInChildren<Room>())
        {
            // Initialize the room list from the list of rooms in the interior
            // environment
            rooms.Add(room);

            // Build the navmesh
            room.GetNavMeshSurface().BuildNavMesh();
        }

        initialized = true;
    }
}
