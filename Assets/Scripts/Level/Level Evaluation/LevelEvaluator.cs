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
    [Header("Level Metadata")]
    public string settingName;
    public int sceneIndex; // Which scene to load

    [Header("Level Generation Config")]
    public int numGhosts = 1;
    public int difficulty = 1;

    protected bool initialized = false;
    protected float progress = 0f;
    public bool InitDone { get { return initialized; } }
    public float InitProgress { get { return progress; } }

    /// <summary>
    /// A list of room references in the evaluator, used primarily by
    /// GetRoomFromPositions.
    /// </summary>
    protected List<Room> rooms;

    /// <summary>
    /// The hunting zones for access
    /// </summary>
    protected List<Room>[] zones;
    public List<Room>[] HuntingZones { private set { } get { return zones; } }

    public List<Room> GetAllRooms()
    {
        return rooms;
    }

    /// <summary>
    /// Call this to randomize the initial values of the evaluator instance
    /// </summary>
    public abstract void RandomizeValues();

    /// <summary>
    /// This method generates and initializes the interior structure of a
    /// level. Implement this level to customize a level type's procedural
    /// generation algorithm. This will also partition an interior into zones
    /// for multiple ghosts.
    /// </summary>
    /// <param name="g">The root object of the interior in the scene 
    /// <param name="zones"></param>
    /// hierarchy.</param>
    /// <returns>An array of "zones" for support for multiple ghosts</returns>
    public abstract List<Room>[] InitializeInterior(GameObject g, int zones);

    /// <summary>
    /// Call this function instead
    /// </summary>
    /// <param name="g">The root object of the interior in the scene 
    /// <param name="zones">The number of hunting zones</param>
    public async virtual void InitializeInteriorAsync(GameObject g, int zones)
    {
        this.zones = InitializeInterior(g, zones);
        initialized = true;
        progress = 1f;
    }

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
            if (room.myCollider.bounds.Contains(globalPosition))
            {
                return room;
            }
        }
        return null;
    }
}
