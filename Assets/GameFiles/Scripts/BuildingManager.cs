using UnityEngine;
using States;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {
    //Game Manager
    public GameManager gm;

    //HUD Manager
    public HUDManager hudm;

    //Resource Data
    [SerializeField]
    private PlayerData playerData;

    //Building Variables
    [SerializeField]
    private GameObject cellIndicator;
    private GameObject visualObject;
    [SerializeField]
    internal Grid grid;
    private byte [,] cellRoadGrid = new byte [100,100]; // 0 = not road, 1 = road
    private int selectedObjectIndex = -1;
    public Material transparent;
    [SerializeField]
    private ObjectsDataBaseSO database;

    private List<Transform> nodes = new List<Transform>();

    void Start() {
        hudm.UpdateLumber(GetLumber());
        hudm.UpdateMetal(GetMetal());
    }

    void Update() {
        if(gm.state == State.Building) {
            if (selectedObjectIndex < 0) return;
            Vector3 mousePosition = gm.MousePosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            gridPosition.z = 0;
            cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
    }

    public bool isAffordable(int id) {
        if (playerData.lumber < database.objectsData[id].cost) {
            Debug.Log("Not Enough Lumber");
            return false;
        }
        return true;
    }

    public  bool isRoadAdjacent() {
        foreach (Transform node in nodes) {
            Vector3Int nodeCell = grid.WorldToCell(node.position);
            if (cellRoadGrid[nodeCell.x+50, nodeCell.y+50] == 1) {
                return true;
            }
        }
        return false;
    }

    public int isRoadAdjacentForRoad() {
        foreach (Transform node in nodes) {
            Vector3Int nodeCell = grid.WorldToCell(node.transform.GetChild(1).position);
            if (cellRoadGrid[nodeCell.x+50, nodeCell.y+50] == 1) {
                return node.GetSiblingIndex();
            }
        }
        return -1;
    }

    public bool StartPlacement(int id) {

        //Clearing the previous building
        Destroy(visualObject, 0);
        nodes.Clear();

        //Checking the id of the building
        selectedObjectIndex = database.objectsData.FindIndex(data => data.id == id);
        if (selectedObjectIndex < 0) {
            Debug.LogError($"No ID found {id}");
            return false;
        }

        //Instantiating the preview building
        cellIndicator.SetActive(true);
        visualObject = Instantiate(database.objectsData[selectedObjectIndex].prefab, cellIndicator.transform);
        visualObject.transform.localPosition = Vector3.zero;
        visualObject.GetComponentInChildren<MeshRenderer>().material = transparent;
        Transform obj = visualObject.transform.GetChild(0);

        // Getting the nodes cell position from the building so that we can check if the cells contain a road and therefore placeable
        for(int i = 0; i < visualObject.transform.GetChild(1).childCount; i++) {
            nodes.Add(visualObject.transform.GetChild(1).GetChild(i));
        }

        //Adding the PlacementValidity script to the building
        var x = obj.gameObject.AddComponent<PlacementValidity>();
        x.gm = gm;

        return true;
    }

    public GameObject PlaceRoad(Vector3 mousePosition, int id, int roadAdjacent) {

        GameObject road = PlaceBuilding(mousePosition, id);
        road.transform.GetChild(1).transform.GetChild(roadAdjacent).gameObject.SetActive(true);
        return road;
    }


    public GameObject PlaceBuilding(Vector3 mousePosition, int id) {
        
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (id == 3) {
            SetCellToRoad(gridPosition);
            AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete);
        } else if (id == 1) {
            gm.unitSlots += 2;
            hudm.UpdateUnitSlots(gm.unitSlots);
            AudioManager.Instance.Play(AudioManager.SoundType.BuildingConstruct);
        } else {
            AudioManager.Instance.Play(AudioManager.SoundType.BuildingConstruct);
        }

        gridPosition.z = 0;
        Debug.Log(id);

        GameObject newObject = Instantiate(database.objectsData[id].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.SetParent(this.transform);
        gm.UpdateNavMesh();
        

        return newObject;
    }

    public void CancelBuilding() {
        Destroy(visualObject, 0);
        nodes.Clear();
        cellIndicator.SetActive(false);
        hudm.GetWorkerPanel().SetActive(false);
    }

    public void SetCellToRoad(Vector3Int gridPosition) {
        cellRoadGrid[gridPosition.x+50, gridPosition.y+50] = 1;
    }

    public void PurchaseBuilding(int id) {
        RemoveLumber(database.objectsData[id].cost);
    }

    // public GameObject PlaceConstructionSite(Vector3 mousePosition, int id) {
    //     Vector3Int gridPosition = grid.WorldToCell(mousePosition);
    //     gridPosition.z = 0;
    //     GameObject newObject = Instantiate(database.objectsData[id].constructionPrefab);
    //     newObject.transform.position = grid.CellToWorld(gridPosition);
    //     newObject.transform.SetParent(this.transform);
    //     gm.UpdateNavMesh();
    //     return newObject;
    // }

    public void AddLumber(int amount) {
        playerData.lumber += amount;
        hudm.UpdateLumber(playerData.lumber);
    }

    public void AddMetal(int amount) {
        playerData.metal += amount;
        hudm.UpdateMetal(playerData.metal);
    }

    public void RemoveLumber(int amount) {
        playerData.lumber -= amount;
        hudm.UpdateLumber(playerData.lumber);
    }

    public void RemoveMetal(int amount) {
        playerData.metal -= amount;
        hudm.UpdateMetal(playerData.metal);
    }

    public int GetLumber() {
        return playerData.lumber;
    }

    public int GetMetal() {
        return playerData.metal;
    }

    public float GetBuildTime(int id) {
        return database.objectsData[id].buildTime;
    }

    public int GetSelectedObjectIndex() {
        return selectedObjectIndex;
    }
}