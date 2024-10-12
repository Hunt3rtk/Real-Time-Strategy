using UnityEngine;
using States;
using System.Collections;

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
    private GameObject mouseIndicator, cellIndicator;
    private GameObject visualObject;
    [SerializeField]
    internal Grid grid;
    private byte [,] cellRoadAdjacency = new byte [100,100]; // 0 = not adjacent, 1 = adjacent, 2 = road tile
    private int selectedObjectIndex = -1;
    public Material transparent;
    [SerializeField]
    private ObjectsDataBaseSO database;

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
            mouseIndicator.transform.position = mousePosition;
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

    public IEnumerator PlaceBuilding(Vector3 mousePosition) {
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //TO-DO Implement Before the placing of the building
        // if (cellRoadAdjacency[gridPosition.x+50, gridPosition.y+50] == 0) {
        //     yield break;
        // }
        if (selectedObjectIndex == 3) {
            SetRoadAdjacencies(gridPosition);
        }
        gridPosition.z = 0;
        Debug.Log(selectedObjectIndex);
        RemoveLumber(database.objectsData[selectedObjectIndex].cost);
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.SetParent(this.transform);
        gm.BakeNavMesh();
        selectedObjectIndex = -1;
        yield return null;
    }

    public void CancelBuilding() {
        Destroy(visualObject, 0);
        mouseIndicator.SetActive(false);
        cellIndicator.SetActive(false);
        hudm.GetBuildingPanel().SetActive(false);
    }

    public bool StartPlacement(int id) {
        Destroy(visualObject, 0);
        selectedObjectIndex = database.objectsData.FindIndex(data => data.id == id);
        if (selectedObjectIndex < 0) {
            Debug.LogError($"No ID found {id}");
            return false;
        }
        cellIndicator.SetActive(true);
        mouseIndicator.SetActive(true);
        visualObject = Instantiate(database.objectsData[selectedObjectIndex].prefab, cellIndicator.transform);
        visualObject.transform.localPosition = Vector3.zero;
        visualObject.GetComponentInChildren<MeshRenderer>().material = transparent;
        Transform obj = visualObject.transform.GetChild(0);
        var x = obj.gameObject.AddComponent<PlacementValidity>();
        x.gm = gm;
        return true;
    }

    public void SetRoadAdjacencies(Vector3Int gridPosition) {
                    cellRoadAdjacency[gridPosition.x+50, gridPosition.y+50] = 2;
            cellRoadAdjacency[gridPosition.x-1+50, gridPosition.y+50] = 1;
            cellRoadAdjacency[gridPosition.x+1+50, gridPosition.y+50] = 1;
            cellRoadAdjacency[gridPosition.x+50, gridPosition.y-1+50] = 1;
            cellRoadAdjacency[gridPosition.x+50, gridPosition.y+1+50] = 1;
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