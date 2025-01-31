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
    /// This method generates and initializes the interior structure of a
    /// level. Implement this level to customize a level type's procedural
    /// generation algorithm.
    /// </summary>
    /// <param name="g">The root object of the interior in the scene 
    /// hierarchy.</param>
    public abstract void GenerateInterior(GameObject g);

    /// <summary>
    /// Takes a position and evaluates which room that point is in.
    /// </summary>
    /// <param name="globalPosition"></param>
    /// <returns>NULL if globalPosition is out of bounds, a reference
    /// to the Room a point is in otherwise.</returns>
    public abstract Room GetRoomFromPosition(Vector3 globalPosition);
}
