using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A class representing the level evaluator for a pre-designed level (Requiring
/// no room generation/procedural generation whatsoever). In this case, all we
/// need to do is build the navmesh.
/// </summary>
[CreateAssetMenu(fileName = "Unnamed Level Evaluator", menuName = "Level Evaluators/Hotel Level Evaluator")]
public class HotelLevelEvaluator : LevelEvaluator
{
    [Header("Hotel Level Config")]
    [SerializeField]
    private int floorCount = 1;
    [SerializeField]
    private int corridorSize = 4;
    [SerializeField]
    private float centralAreaWidth = 0;
    [SerializeField]
    private float corridorWidth = 0;
    [SerializeField]
    private float roomUnitSize = 0;
    [SerializeField]
    private float floorVerticalDistance = 0;
    [SerializeField]
    private float corridorHeight = 0;

    [SerializeField]
    private List<GameObject> tileWidth1Prefabs;
    [SerializeField]
    private List<GameObject> tileWidth2Prefabs;
    [SerializeField]
    private List<GameObject> tileWidth3Prefabs;
    [SerializeField]
    private GameObject stairwellBase;
    [SerializeField]
    private GameObject stairwellMid;
    [SerializeField]
    private GameObject stairwellBlocked;
    [SerializeField]
    private GameObject corridorPiece;
    [SerializeField]
    private GameObject corridorPrefab;
    [SerializeField]
    private GameObject centerPrefab;

    public override List<Room>[] InitializeInterior(GameObject g, int zones)
    {
        rooms = new List<Room>();
        List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        List<Room>[] zoneArr = new List<Room>[floorCount + 1];

        // For each floor
        for (int j = 0; j < floorCount + 1; ++j)
        {
            // Compute the height at which things should be generated
            float y_val = j * floorVerticalDistance;

            // Create the center object
            Room centerObject = Instantiate(centerPrefab, g.transform).GetComponent<Room>();
            centerObject.transform.localPosition = new Vector3(0, y_val, 0);
            surfaces.Add(centerObject.gameObject.GetComponentInChildren<NavMeshSurface>());

            // Corridor generation code (left side)
            GameObject corr = Instantiate(corridorPrefab, g.transform);
            corr.transform.localPosition = new Vector3(centralAreaWidth, y_val, 0);

            BoxCollider coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);

            RoomLightInteractable lights = corr.GetComponent<RoomLightInteractable>();

            for (int i = 0; i < corridorSize; ++i)
            {
                GameObject r = Instantiate(corridorPiece, corr.transform);
                r.transform.localPosition = new Vector3(roomUnitSize * i, 0, 0);

                foreach (MeshRenderer m in r.GetComponentsInChildren<MeshRenderer>())
                {
                    lights.AddLitMesh(m);
                }
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
            }
            GameObject stairwell = Instantiate(stairwellBase, g.transform);
            stairwell.transform.localPosition = new Vector3(roomUnitSize * corridorSize + (roomUnitSize * 0.5f), 0, 0);
            Room.SetRoomAdjacency(centerObject, corr.GetComponent<Room>());

            // Generate the left north side
            List<int> roomSizes = new List<int>();
            Vector3 offset = new Vector3(centralAreaWidth, y_val, corridorWidth * 0.5f + roomUnitSize * 0.5f);
            for (int k = 0; k < corridorSize; ++k)
            {
                GameObject r = Instantiate(tileWidth1Prefabs[0], g.transform);
                r.transform.localPosition = offset + new Vector3(k *  roomUnitSize, 0, 0);

                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
            }

            // Generate the left south side

            // Corridor generation code (right side)
            corr = Instantiate(corridorPrefab, g.transform);
            corr.transform.localPosition = new Vector3(-centralAreaWidth, y_val, 0);

            coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(-roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);

            lights = corr.GetComponent<RoomLightInteractable>();

            for (int i = 0; i < corridorSize; ++i)
            {
                GameObject r = Instantiate(corridorPiece, corr.transform);
                r.transform.localPosition = new Vector3(-roomUnitSize * i, 0, 0);

                foreach (MeshRenderer m in r.GetComponentsInChildren<MeshRenderer>())
                {
                    lights.AddLitMesh(m);
                }
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
            }
            stairwell = Instantiate(stairwellBase, g.transform);
            stairwell.transform.Rotate(new Vector3(0, 180, 0));
            stairwell.transform.localPosition = new Vector3(-roomUnitSize * corridorSize - (roomUnitSize * 0.5f), 0, 0);
            Room.SetRoomAdjacency(centerObject, corr.GetComponent<Room>());

        }

        foreach (NavMeshSurface surf in surfaces)
        {
            // Build the navmesh
            surf.BuildNavMesh();
        }

        // TODO: List<Room>[] zones = new List<Room>[zones];
        return zoneArr;
    }
}
