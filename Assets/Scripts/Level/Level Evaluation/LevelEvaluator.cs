using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Level Evaluator class is the basis for algorithms involving generating
/// the level and identifying which room in the level a point is located in.
/// The idea is that each level type will implement level generation and (for 
/// optimization purposes) identifying which room a point falls in.
/// </summary>
public abstract class LevelEvaluator : ScriptableObject
{
    /// <summary>
    /// A list of room references in the evaluator, used primarily by
    /// GetRoomFromPositions.
    /// </summary>
    protected List<Room> rooms;
    public List<Room> GetAllRooms()
    {
        return rooms;
    }

    /// <summary>
    /// A boolean representing whether a level has been initialized or not
    /// </summary>
    protected bool initialized = false;

    /// <summary>
    /// This method generates and initializes the interior structure of a
    /// level. Implement this level to customize a level type's procedural
    /// generation algorithm.
    /// </summary>
    /// <param name="g">The root object of the interior in the scene 
    /// hierarchy.</param>
    public abstract void InitializeInterior(GameObject g);

    /// <summary>
    /// Takes a position and evaluates which room that point is in. By default
    /// this function simply checks the list of room references for a room which
    /// contains the argument.
    /// </summary>
    /// <param name="globalPosition"></param>
    /// <returns>NULL if globalPosition is out of bounds, a reference
    /// to the Room a point is in otherwise.</returns>
    public Room GetRoomFromPosition(Vector3 globalPosition)
    {
        foreach (Room room in rooms)
        {
            if (room.GetBoundingBox().Contains(globalPosition))
            {
                return room;
            }
        }
        return null;
    }
}
