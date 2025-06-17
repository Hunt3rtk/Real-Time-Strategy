using UnityEngine;
using States;
using System.Collections;
using System.Collections.Generic;
using static AudioManager;

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
    public GameObject visualObject;
    [SerializeField]
    internal Grid grid;
    private byte [,] cellRoadGrid = new byte [100,100]; // 0 = not road, 1 = road
    private int selectedObjectIndex = -1;
    public Material transparent;
    public Material flashColor;

    [SerializeField]
    private ObjectsDataBaseSO database;

    private BuildingAnimationPlayer buildingAnimationPlayer;

    private List<Transform> nodes = new List<Transform>();

    void Start() {
        hudm.UpdateLumber(GetLumber());
        hudm.UpdateGold(GetGold());

        buildingAnimationPlayer = GetComponent<BuildingAnimationPlayer>();
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

    public List<int> isRoadAdjacentForRoad() {
        List<int> adjacentIndices = new List<int>();
        foreach (Transform node in nodes) {
            Vector3Int nodeCell = grid.WorldToCell(node.transform.GetChild(1).position);
            if (cellRoadGrid[nodeCell.x+50, nodeCell.y+50] == 1) {
                adjacentIndices.Add(node.GetSiblingIndex());
            }
        }
        return adjacentIndices;
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

        //Checking if the player can afford the building
        if (!isAffordable(id)) {
            StartCoroutine(AudioManager.Instance.Play(SoundType.PurchaseFail));
            return false;
        }

        //Instantiating the preview building
        cellIndicator.SetActive(true);
        visualObject = Instantiate(database.objectsData[selectedObjectIndex].prefab, cellIndicator.transform);
        visualObject.transform.localPosition = Vector3.zero;
        List<Material> mats = new List<Material>
        {
            transparent,
            flashColor
        };
        visualObject.GetComponentInChildren<MeshRenderer>().SetMaterials(mats);
        visualObject.AddComponent<BuildingFlash>();
        Transform obj = visualObject.transform.GetChild(0);

        if (id == 1) {
            int unitSlots = FindAnyObjectByType<GameManager>().unitSlots -= 2;
            FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);
        }
        

        // Getting the nodes cell position from the building so that we can check if the cells contain a road and therefore placeable
        for (int i = 0; i < visualObject.transform.GetChild(1).childCount; i++) {
            nodes.Add(visualObject.transform.GetChild(1).GetChild(i));
        }

        //Adding the PlacementValidity script to the building
        var x = obj.gameObject.AddComponent<PlacementValidity>();
        x.gm = gm;

        return true;
    }

    public GameObject PlaceRoad(Vector3 mousePosition, int id, List<int> roadAdjacents) {

        GameObject road = PlaceBuilding(mousePosition, id);

        foreach (int roadAdjacent in roadAdjacents) {
            road.transform.Find("Nodes").transform.GetChild(roadAdjacent).gameObject.SetActive(true);
        }
        
        return road;
    }


    public GameObject PlaceBuilding(Vector3 mousePosition, int id) {
        
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (id == 3) {
            SetCellToRoad(gridPosition);
            StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingPlaced));

        } else if (id == 1) {
            StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingPlaced));
            StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingConstruct));

        } else {
            StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingPlaced));
            StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingConstruct));

        }

        gridPosition.z = 0;
        Debug.Log(id);

        GameObject newObject = Instantiate(database.objectsData[id].prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.SetParent(this.transform);
        gm.UpdateNavMesh();

        buildingAnimationPlayer.PlaceEffect.particleSystem = newObject.transform.Find("BuildingPlaced").GetComponent<ParticleSystem>();
        buildingAnimationPlayer.PlayPlace();

        PurchaseBuilding(id);

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

    public void AddLumber(int amount) {
        playerData.lumber += amount;
        hudm.UpdateLumber(playerData.lumber);
    }

    public void AddGold(int amount) {
        playerData.gold += amount;
        hudm.UpdateGold(playerData.gold);
    }

    public void RemoveLumber(int amount) {
        playerData.lumber -= amount;
        hudm.UpdateLumber(playerData.lumber);
    }

    public void RemoveGold(int amount) {
        playerData.gold -= amount;
        hudm.UpdateGold(playerData.gold);
    }

    public int GetLumber() {
        return playerData.lumber;
    }

    public int GetGold() {
        return playerData.gold;
    }

    public float GetBuildTime(int id) {
        return database.objectsData[id].buildTime;
    }

    public int GetSelectedObjectIndex() {
        return selectedObjectIndex;
    }
}