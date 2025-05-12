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

    public override void RandomizeValues()
    {

    }

    public override List<Room>[] InitializeInterior(GameObject g, int zones)
    {
        rooms = new List<Room>();
        List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        List<Room>[] zoneArr = new List<Room>[floorCount + 1];
        for (int i = 0; i < floorCount + 1; ++i)
        {
            zoneArr[i] = new List<Room>();
        }

        // For each floor
        for (int j = 0; j < floorCount + 1; ++j)
        {
            // Compute the height at which things should be generated
            float y_val = j * floorVerticalDistance;

            // Create the center object
            Room centerObject = Instantiate(centerPrefab, g.transform).GetComponent<Room>();
            centerObject.transform.localPosition = new Vector3(0, y_val, 0);
            surfaces.Add(centerObject.gameObject.GetComponentInChildren<NavMeshSurface>());
            zoneArr[j].Add(centerObject);
            rooms.Add(centerObject);

            // Corridor generation code (left side)
            GameObject corr = Instantiate(corridorPrefab, g.transform);
            corr.transform.localPosition = new Vector3(centralAreaWidth, y_val, 0);

            BoxCollider coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);

            RoomLightInteractable lights = corr.GetComponent<RoomLightInteractable>();
            Room corrRoom = corr.GetComponent<Room>();
            corrRoom.ResetBounds();
            zoneArr[j].Add(corrRoom);
            rooms.Add(corrRoom);

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
            Room.SetRoomAdjacency(corrRoom, centerObject);

            // Generate the left north side
            List<int> roomSizes = new List<int>();
            int sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add( rsize );
                sum += rsize;
            }
            Vector3 offset = new Vector3(centralAreaWidth, y_val, corridorWidth * 0.5f + roomUnitSize * 0.5f);
            int k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                if (pickedSize == 1)
                {
                    r = Instantiate(tileWidth1Prefabs[0], g.transform);
                } else if (pickedSize == 2)
                {
                    r = Instantiate(tileWidth2Prefabs[0], g.transform);
                } else
                {
                    r = Instantiate(tileWidth3Prefabs[0], g.transform);
                }
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) *  roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);

            }

            // Generate the left south side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(centralAreaWidth, y_val, -corridorWidth * 0.5f - roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                if (pickedSize == 1)
                {
                    r = Instantiate(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    r = Instantiate(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    r = Instantiate(tileWidth3Prefabs[0], g.transform);
                }
                r.transform.localScale = new Vector3(1, 1, -1); 
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

            // Corridor generation code (right side)
            corr = Instantiate(corridorPrefab, g.transform);
            corr.transform.localPosition = new Vector3(-centralAreaWidth, y_val, 0);

            coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(-roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);
            
            lights = corr.GetComponent<RoomLightInteractable>();
            corrRoom = corr.GetComponent<Room>();
            corrRoom.ResetBounds();
            zoneArr[j].Add(corrRoom);
            rooms.Add(corrRoom);

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
            Room.SetRoomAdjacency(corrRoom, centerObject);

            // Generate the left north side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(-centralAreaWidth, y_val, corridorWidth * 0.5f + roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                if (pickedSize == 1)
                {
                    r = Instantiate(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    r = Instantiate(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    r = Instantiate(tileWidth3Prefabs[0], g.transform);
                }
                r.transform.localScale = new Vector3(-1, 1, 1);
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * -roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

            // Generate the left south side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(-centralAreaWidth, y_val, -corridorWidth * 0.5f - roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                if (pickedSize == 1)
                {
                    r = Instantiate(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    r = Instantiate(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    r = Instantiate(tileWidth3Prefabs[0], g.transform);
                }
                r.transform.localScale = new Vector3(-1, 1, -1);
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * -roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

        }

        surfaces[0].BuildNavMesh();

        // TODO: List<Room>[] zones = new List<Room>[zones];
        return zoneArr;
    }

    public override async void InitializeInteriorAsync(GameObject g, int zones)
    {
        AsyncInstantiateOperation<GameObject> asyncInst;
        // 1 for the centerpiece
        // On each side 1 for the corridor gameobj + stairwell
        // 3 * corridor size for corridor + room pieces
        float interval = 0.5f / (floorCount * (1f + 2f * (2f + 3f * corridorSize)));
        Debug.Log(1f / interval);

        rooms = new List<Room>();
        List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        List<Room>[] zoneArr = new List<Room>[floorCount + 1];
        for (int i = 0; i < floorCount + 1; ++i)
        {
            zoneArr[i] = new List<Room>();
        }

        // For each floor
        for (int j = 0; j < floorCount + 1; ++j)
        {
            // Compute the height at which things should be generated
            float y_val = j * floorVerticalDistance;

            // Create the center object
            asyncInst = InstantiateAsync<GameObject>(centerPrefab, g.transform);
            await asyncInst;
            progress += interval;
            Room centerObject = asyncInst.Result[0].GetComponent<Room>();
            centerObject.transform.localPosition = new Vector3(0, y_val, 0);
            surfaces.Add(centerObject.gameObject.GetComponentInChildren<NavMeshSurface>());
            zoneArr[j].Add(centerObject);
            rooms.Add(centerObject);

            // Corridor generation code (left side)
            asyncInst = InstantiateAsync<GameObject>(corridorPrefab, g.transform);
            await asyncInst;
            progress += interval;
            GameObject corr = asyncInst.Result[0];
            corr.transform.localPosition = new Vector3(centralAreaWidth, y_val, 0);

            BoxCollider coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);

            RoomLightInteractable lights = corr.GetComponent<RoomLightInteractable>();
            Room corrRoom = corr.GetComponent<Room>();
            corrRoom.ResetBounds();
            zoneArr[j].Add(corrRoom);
            rooms.Add(corrRoom);

            for (int i = 0; i < corridorSize; ++i)
            {
                asyncInst = InstantiateAsync<GameObject>(corridorPiece, corr.transform);
                await asyncInst;
                progress += interval;
                GameObject r = asyncInst.Result[0];
                r.transform.localPosition = new Vector3(roomUnitSize * i, 0, 0);

                foreach (MeshRenderer m in r.GetComponentsInChildren<MeshRenderer>())
                {
                    lights.AddLitMesh(m);
                }
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
            }
            asyncInst = InstantiateAsync<GameObject>(stairwellBase, g.transform);
            await asyncInst;
            progress += interval;
            GameObject stairwell = asyncInst.Result[0];
            stairwell.transform.localPosition = new Vector3(roomUnitSize * corridorSize + (roomUnitSize * 0.5f), 0, 0);
            Room.SetRoomAdjacency(corrRoom, centerObject);

            // Generate the left north side
            List<int> roomSizes = new List<int>();
            int sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            Vector3 offset = new Vector3(centralAreaWidth, y_val, corridorWidth * 0.5f + roomUnitSize * 0.5f);
            int k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                AsyncInstantiateOperation<GameObject> op;
                if (pickedSize == 1)
                {
                    op = InstantiateAsync<GameObject>(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    op = InstantiateAsync<GameObject>(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    op = InstantiateAsync<GameObject>(tileWidth3Prefabs[0], g.transform);
                }
                await op;
                progress += interval * pickedSize;
                r = op.Result[0];
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);

            }

            // Generate the left south side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(centralAreaWidth, y_val, -corridorWidth * 0.5f - roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                AsyncInstantiateOperation<GameObject> op;
                if (pickedSize == 1)
                {
                    op = InstantiateAsync<GameObject>(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    op = InstantiateAsync<GameObject>(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    op = InstantiateAsync<GameObject>(tileWidth3Prefabs[0], g.transform);
                }
                await op;
                progress += interval * pickedSize;
                r = op.Result[0];
                r.transform.localScale = new Vector3(1, 1, -1);
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

            // Corridor generation code (right side)
            asyncInst = InstantiateAsync(corridorPrefab, g.transform);
            await asyncInst;
            progress += interval;
            corr = asyncInst.Result[0];
            corr.transform.localPosition = new Vector3(-centralAreaWidth, y_val, 0);

            coll = corr.GetComponent<BoxCollider>();
            coll.center = new Vector3(-roomUnitSize * 0.5f * (corridorSize - 1), corridorHeight * 0.5f, 0);
            coll.size = new Vector3(roomUnitSize * corridorSize, corridorHeight, corridorWidth);

            lights = corr.GetComponent<RoomLightInteractable>();
            corrRoom = corr.GetComponent<Room>();
            corrRoom.ResetBounds();
            zoneArr[j].Add(corrRoom);
            rooms.Add(corrRoom);

            for (int i = 0; i < corridorSize; ++i)
            {
                asyncInst = InstantiateAsync(corridorPiece, corr.transform);
                await asyncInst;
                progress += interval;
                GameObject r = asyncInst.Result[0];
                r.transform.localPosition = new Vector3(-roomUnitSize * i, 0, 0);

                foreach (MeshRenderer m in r.GetComponentsInChildren<MeshRenderer>())
                {
                    lights.AddLitMesh(m);
                }
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
            }
            asyncInst = InstantiateAsync(stairwellBase, g.transform);
            await asyncInst;
            progress += interval;
            stairwell = asyncInst.Result[0];
            stairwell.transform.Rotate(new Vector3(0, 180, 0));
            stairwell.transform.localPosition = new Vector3(-roomUnitSize * corridorSize - (roomUnitSize * 0.5f), 0, 0);
            Room.SetRoomAdjacency(corrRoom, centerObject);

            // Generate the left north side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(-centralAreaWidth, y_val, corridorWidth * 0.5f + roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                AsyncInstantiateOperation<GameObject> op;
                if (pickedSize == 1)
                {
                    op = InstantiateAsync<GameObject>(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    op = InstantiateAsync<GameObject>(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    op = InstantiateAsync<GameObject>(tileWidth3Prefabs[0], g.transform);
                }
                await op;
                progress += interval * pickedSize;
                r = op.Result[0];
                r.transform.localScale = new Vector3(-1, 1, 1);
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * -roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

            // Generate the left south side
            roomSizes = new List<int>();
            sum = 0;
            while (sum < corridorSize)
            {
                int rsize = Random.Range(1, 4);
                while (sum + rsize > corridorSize) { rsize = Random.Range(1, 4); }
                roomSizes.Add(rsize);
                sum += rsize;
            }
            offset = new Vector3(-centralAreaWidth, y_val, -corridorWidth * 0.5f - roomUnitSize * 0.5f);
            k = 0;
            foreach (int pickedSize in roomSizes)
            {
                GameObject r;
                AsyncInstantiateOperation<GameObject> op;
                if (pickedSize == 1)
                {
                    op = InstantiateAsync<GameObject>(tileWidth1Prefabs[0], g.transform);
                }
                else if (pickedSize == 2)
                {
                    op = InstantiateAsync<GameObject>(tileWidth2Prefabs[0], g.transform);
                }
                else
                {
                    op = InstantiateAsync<GameObject>(tileWidth3Prefabs[0], g.transform);
                }
                await op;
                progress += interval * pickedSize;
                r = op.Result[0];
                r.transform.localScale = new Vector3(-1, 1, -1);
                r.transform.localPosition = offset + new Vector3((k + pickedSize - 1) * -roomUnitSize, 0, 0);
                k += pickedSize;
                surfaces.Add(r.GetComponentInChildren<NavMeshSurface>());
                zoneArr[j].Add(r.GetComponent<Room>());
                rooms.Add(r.GetComponent<Room>());
                r.GetComponent<Room>().ResetBounds();
                Room.SetRoomAdjacency(r.GetComponent<Room>(), corrRoom);
            }

        }

        surfaces[0].BuildNavMesh();

        // TODO: List<Room>[] zones = new List<Room>[zones];
        this.zones = zoneArr;
        initialized = true;
    }
}
