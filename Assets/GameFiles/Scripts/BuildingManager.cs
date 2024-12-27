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

    public bool isAffordable() {
        if (playerData.lumber < database.objectsData[selectedObjectIndex].cost) {
            Debug.Log("Not Enough Lumber");
            return false;
        }
        return true;
    }

    public bool isRoadAdjacent() {
        foreach (Transform node in nodes) {
            Vector3Int nodeCell = grid.WorldToCell(node.position);
            if (cellRoadGrid[nodeCell.x+50, nodeCell.y+50] == 1) {
                return true;
            }
        }
        return false;
    }

    public bool StartPlacement(int id) {
        Destroy(visualObject, 0);
        selectedObjectIndex = database.objectsData.FindIndex(data => data.id == id);
        if (selectedObjectIndex < 0) {
            Debug.LogError($"No ID found {id}");
            return false;
        }
        cellIndicator.SetActive(true);
        visualObject = Instantiate(database.objectsData[selectedObjectIndex].prefab, cellIndicator.transform);
        visualObject.transform.localPosition = Vector3.zero;
        visualObject.GetComponentInChildren<MeshRenderer>().material = transparent;
        Transform obj = visualObject.transform.GetChild(0);

        // Getting the nodes cell position from the building so that we can check if the cells contain a road and therefore placeable
        for(int i = 0; i < visualObject.transform.GetChild(1).childCount; i++) {
            nodes.Add(visualObject.transform.GetChild(1).GetChild(i));
        }

        var x = obj.gameObject.AddComponent<PlacementValidity>();
        x.gm = gm;
        return true;
    }


    public IEnumerator PlaceBuilding(Vector3 mousePosition) {
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (selectedObjectIndex == 3) {
            SetCellToRoad(gridPosition);
        }
        gridPosition.z = 0;
        Debug.Log(selectedObjectIndex);
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.SetParent(this.transform);
        gm.UpdateNavMesh();
        selectedObjectIndex = -1;
        yield return null;
    }

    public void CancelBuilding() {
        Destroy(visualObject, 0);
        nodes.Clear();
        cellIndicator.SetActive(false);
        hudm.GetBuildingPanel().SetActive(false);
    }

    public void SetCellToRoad(Vector3Int gridPosition) {
        cellRoadGrid[gridPosition.x+50, gridPosition.y+50] = 1;
    }

    public void PurchaseBuilding() {
        RemoveLumber(database.objectsData[selectedObjectIndex].cost);
    }

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

    public float GetBuildTime() {
        return database.objectsData[selectedObjectIndex].buildTime;
    }
}