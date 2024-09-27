using UnityEngine;
using States;

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
    private Grid grid;
    private int selectedObjectIndex = -1;
    public Material transparent;
    [SerializeField]
    private ObjectsDataBaseSO database;

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

    public void PlaceBuilding(Vector3 mousePosition) {
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        gridPosition.z = 0;
        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.SetParent(this.transform);
        gm.BakeNavMesh();
    }

    public void CancelBuilding() {
        selectedObjectIndex = -1;
        Destroy(visualObject, 0);
        mouseIndicator.SetActive(false);
        cellIndicator.SetActive(false);
        hudm.GetBuildingPanel().SetActive(false);
    }

    public void StartPlacement(int id) {
        Destroy(visualObject, 0);
        selectedObjectIndex = database.objectsData.FindIndex(data => data.id == id);
        if (selectedObjectIndex < 0) {
            Debug.LogError($"No ID found {id}");
            return;
        }
        cellIndicator.SetActive(true);
        mouseIndicator.SetActive(true);
        visualObject = Instantiate(database.objectsData[selectedObjectIndex].prefab, cellIndicator.transform);
        visualObject.transform.localPosition = Vector3.zero;
        visualObject.GetComponentInChildren<MeshRenderer>().material = transparent;
        Transform obj = visualObject.transform.GetChild(0);
        var x = obj.gameObject.AddComponent<PlacementValidity>();
        x.gm = gm;
    }

    public void AddLumber(int amount) {
        playerData.lumber += amount;
        string x = playerData.lumber.ToString();
        hudm.UpdateLumber(x);
    }

    public void AddMetal(int amount) {
        playerData.metal += amount;
        string x = playerData.metal.ToString();
        hudm.UpdateMetal(x);
    }

    public void RemoveLumber(int amount) {
        playerData.lumber -= amount;
        string x = playerData.lumber.ToString();
        hudm.UpdateLumber(x);
    }

    public void RemoveMetal(int amount) {
        playerData.metal -= amount;
        string x = playerData.metal.ToString();
        hudm.UpdateMetal(x);
    }
}