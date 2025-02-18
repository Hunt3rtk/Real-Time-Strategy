using System.Collections.Generic;
using UnityEngine;
using States;
using Unity.AI.Navigation;
using System.Collections;
using UnityEngine.AI;
using TMPro;
using static AudioManager;
//using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    //Selected Units and their HUD Indicators
    public List<Unit> selectedUnits;

    //Number of Units
    public int unitCount = 0;
    public int unitSlots = 2;

    //Indicator Prefab
    public GameObject selectedIndicator;

    //Indicators In Scene
    private List<GameObject> indicators;

    [SerializeField]
    private HUDManager hudm;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private BuildingManager buildingManager;

    [SerializeField]
    internal Transform home;

    [SerializeField]
    private UnitDataBase units;

    private GameObject panelObject;

    [SerializeField]
    private LayerMask roadLayerMask;

    //State
    public State state;

    /* Enable Disable */

    //On Enable Set selected units and enable gameplay state
    private void OnEnable() {
        state = States.State.Gameplay;
        selectedUnits = new List<Unit>();
        indicators = new List<GameObject>();
        inputManager.Enable();
        inputManager.EnableGameplay();
        buildingManager.SetCellToRoad(buildingManager.grid.WorldToCell(home.position));
        UpdateNavMesh();
    }

    //On Disable Disable Gameplay state
    private void OnDisable() {
        inputManager.DisableGameplay();
    }
    /*----------------------------------*/


    /* State Setting */

    //Set State to Gameplay
    public void SetStateGameplay() {
        ClearSelected();
        switch(state) {
            case State.Building:
                inputManager.DisableBuilding();
                state = State.Gameplay;
                inputManager.EnableGameplay();
                break;
            case State.Pause:
                state = State.Gameplay;
                inputManager.EnableGameplay();
                break;
            case State.Gameplay:
            default:
                break;
            }
    }

    //Set State to Building
    public void SetStateBuilding() { 
        switch(state) {
            case State.Gameplay:
                inputManager.DisableGameplay();
                state = State.Building;
                inputManager.EnableBuilding();
                //hudm.GetBuildingPanel().SetActive(true);
                break;
            case State.Pause:
                state = State.Building;
                inputManager.EnableBuilding();
                //hudm.GetBuildingPanel().SetActive(true);
                break;
            case State.Building:
            default:
                break;
        }
    }
    /*----------------------------------*/


    /* Input Processes */

    //Activated Select
    //Finds Tag and Proceeds Accordingly
    public void ActivateSelect() {
        // Selection logic (simplified for example purposes)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, roadLayerMask)) {

            GameObject target = hit.collider.gameObject;
            Unit unit;

            hudm.DeactivateAllPanels();

            switch (target.tag) {
                //Enemy Base
                case "EnemyBase":
                    foreach (Unit ally in selectedUnits) {
                        AudioManager.Instance.Play(SoundType.Command);
                        ally.AttackStandAlone(hit.collider);
                    }
                    break;
                //Enemy Unit
                case "EnemyUnit":
                    foreach (Unit ally in selectedUnits) {
                        AudioManager.Instance.Play(SoundType.Command);
                        ally.AttackStandAlone(hit.collider);
                    }
                    break;
                //Base
                case "Base":
                    ClearSelected();
                    panelObject = target;
                    hudm.ActivateBasePanel();
                    break;
                //Building
                case "Building":
                foreach (Worker worker in selectedUnits) {
                    if (worker == null) continue;
                    ActivateRepair(worker, target.GetComponent<Building>());
                    break;
                }
                    break;
                //Barracks
                case "Barracks":
                    ClearSelected();
                    panelObject = target;
                    hudm.ActivateBarracksPanel();
                    break;
                //Construction Site
                case "Construction":
                    foreach (Worker worker in selectedUnits) {
                        if (worker == null) continue;
                        ActivateRepair(worker, target.transform.parent.GetChild(0).GetComponent<Building>());
                        break;
                    }
                    break;
                //Unit
                case "Unit":
                case "Worker":
                    AudioManager.Instance.Play(SoundType.Select);
                    unit = hit.collider.GetComponentInParent<Unit>();
                    hudm.ActivateUnitPanel(unit);
                    ClearSelected();
                    selectedUnits.Add(unit);
                    AddIndicator(unit);
                    break;
                case "Tree":
                    foreach (Worker worker in selectedUnits) {
                        if (worker == null) continue;
                        AudioManager.Instance.Play(SoundType.Command);
                        worker.StartChop(target.GetComponent<Tree>());
                    }
                    break;
                case "Mine":
                    foreach (Worker worker in selectedUnits) {
                        if (worker == null) continue;
                        AudioManager.Instance.Play(SoundType.Command);
                        worker.StartMine(target.GetComponent<Mine>());
                    }
                    break;
                case "UI":
                    break;
                //Ground
                case "Ground":
                //Everything Else
                default:
                    foreach (Unit x in selectedUnits) {
                        hudm.DeactivateUnitPanel(x);
                    }
                        ClearSelected();
                        break;
            }
        }
    }

    //Activated Shift Select
    //only for units
    public void ActivateMultipleSelect() {
        // Selection logic (simplified for example purposes)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            try {
                Unit unit = hit.collider.GetComponentInParent<Unit>();
                if (unit.gameObject.layer != 8) {
                    AudioManager.Instance.Play(SoundType.Select);
                    selectedUnits.Add(unit);
                    AddIndicator(unit);
                }
            } catch {
                return;
            }
        }
    }

    //Activated Move Units
    public void ActivateMove() {
         // Move command
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            foreach (Unit unit in selectedUnits) {
                if (unit == null) continue;
                AudioManager.Instance.Play(SoundType.Command);
                unit.MoveStandAlone(hit.point);
            }
        }
    }

    //Activates Building
    public void ActivateBuilding(int id) {
        if (!buildingManager.StartPlacement(id)) return;
        SetStateBuilding();
    }

    //Activating Cancel Building and Sets State to Gameplay
    public void ActivateBuildingCancel() {
        buildingManager.CancelBuilding();
        SetStateGameplay();
    }

    //Activates Placing a Building
    public void ActivatePlaceBuilding(Vector3 mousePosition) {

        int id = buildingManager.GetSelectedObjectIndex();

        if(!buildingManager.isAffordable(id)) return;

        if (!buildingManager.isRoadAdjacent()) return;

        if (id == 3) {
           buildingManager.PlaceBuilding(mousePosition, id);
           return;
        }

        Worker worker = null;
        foreach (Worker unit in selectedUnits) {
            if (unit == null) continue;
            worker = unit;
            break;
        }

        ActivateBuildingCancel();

        AudioManager.Instance.Play(SoundType.Command);
        worker.StartConstruct(mousePosition, id);
    }

    //Activate Repair
    private void ActivateRepair(Worker worker, Building building) {
        AudioManager.Instance.Play(SoundType.Command);
        worker.StartRepair(building);
    }

    //Activate Unit Purchase
    public void ActivateUnitPurchase(int id) {
        StartCoroutine(UnitPurchase(id, panelObject.transform));
    }

    // Unit Purchase Coroutine
    public IEnumerator UnitPurchase(int id, Transform building) {

        if(panelObject.GetComponent<Training>().isTraining()) yield break;

        if(!unitAffordable(id)) yield break;

        if(unitCount >= unitSlots) yield break;

        buildingManager.RemoveMetal(units.unitDatas[id].cost);
    
        yield return WaitUnitTime(id);

        GameObject newUnit = Instantiate(units.unitDatas[id].prefab);
        Unit unit = newUnit.GetComponent<Unit>();

        NavMeshHit hit;
        NavMesh.SamplePosition(building.position, out hit, 100f, NavMesh.AllAreas);
        unit.agent.Warp(new Vector3(hit.position.x, hit.position.y+1f, hit.position.z));

        if (id == 0) {
            Worker newWorker = newUnit.GetComponent<Worker>();
            newWorker.home = this.home;
            newWorker.bm = buildingManager;
        }

        hudm.UpdateUnitCount(++unitCount);
    }

    //Toggles Ability to Place
    public void TogglePlace(int collisions) {
        if (collisions > 2) {
            inputManager.DisablePlace();
        } else {
            inputManager.EnablePlace();
        }
    }

    //Waits for the Unit Time
    private IEnumerator WaitUnitTime(int id) {
        yield return panelObject.GetComponent<Training>().StartTraining(units.unitDatas[id].time);
    }

    private bool unitAffordable(int id) {
        if(units.unitDatas[id].cost > buildingManager.GetMetal()) return false;
        return true;
    }

    /*----------------------------------*/


    /* HUD Indicator Methods */

    public void ClearSelected() {
        selectedUnits.Clear();
        DeleteIndicators();
    }

    //Adds HUD indicator for Selected Units
    private void AddIndicator(Unit unit) {
        GameObject ind = Instantiate(selectedIndicator);
        ind.GetComponent<WorldToScreenFollow>().unit = unit;
        ind.transform.parent = hudm.gameObject.transform;
        indicators.Add(ind);
    }

    //Deletes Indicator for Selected Units
    private void DeleteIndicators() {
        foreach (GameObject ind in indicators) {
            Destroy(ind);
        }
    }

    //Gets MousePosition from Input Manager
    public Vector3 MousePosition() {
        return inputManager.GetSelectedMapPosition();
    }
    /*----------------------------------*/


    /* Nav Mesh For Unit Traversal */
    public void UpdateNavMesh() {
        GetComponent<NavMeshSurface>().UpdateNavMesh(GetComponent<NavMeshSurface>().navMeshData);
    }
    /*----------------------------------*/
}

