using System.Collections.Generic;
using UnityEngine;
using States;
using Unity.AI.Navigation;
using System.Collections;

public class GameManager : MonoBehaviour
{

    //Selected Units and their HUD Indicators
    public List<Unit> selectedUnits;

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

    //State
    public State state;


    /* Enable Disable */

    //On Enable Set selected units and enable gameplay state
    private void OnEnable() {
        state = State.Gameplay;
        selectedUnits = new List<Unit>();
        indicators = new List<GameObject>();
        inputManager.Enable();
        inputManager.EnableGameplay();
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
                hudm.GetBuildingPanel().SetActive(true);
                break;
            case State.Pause:
                state = State.Building;
                inputManager.EnableBuilding();
                hudm.GetBuildingPanel().SetActive(true);
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

        if (Physics.Raycast(ray, out hit)) {

            GameObject target = hit.collider.gameObject;
            Unit unit;

            switch (target.tag) {
                //Enemy Base
                case "EnemyBase":
                    break;
                //Enemy Unit
                case "EnemyUnit":
                    unit = hit.collider.GetComponentInParent<Unit>();
                    foreach (Unit ally in selectedUnits) {
                        StartCoroutine(ally.Attack(unit));
                    }
                    break;
                //Base
                case "Base":
                    hudm.ActivateBasePanel();
                    break;
                //Unit
                case "Unit":
                    unit = hit.collider.GetComponentInParent<Unit>();
                    hudm.ActivateUnitPanel(unit);
                    ClearSelected();
                    selectedUnits.Add(unit);
                    AddIndicator(unit);
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
                unit.Move(hit.point);
            }
        }
    }

    //Activates Building
    public void ActivateBuilding(int id) {
        if (!buildingManager.StartPlacement(id)) return;
        SetStateBuilding();
    }

    //Activates Placing a Building
    public IEnumerator ActivatePlaceBuilding(Vector3 mousePosition) {
        if(!buildingManager.isAffordable()) yield break;
        Worker worker = null;
        foreach (Worker unit in selectedUnits) {
            worker = unit;
            break;
        }
        yield return worker.Construct(mousePosition);
        yield return WaitBuildTime();
        worker.gameObject.SetActive(true);
        yield return buildingManager.PlaceBuilding(mousePosition);
        SetStateGameplay();
    }

    //Activating Cancel Building and Sets State to Gameplay
    public void ActivateCancel() {
        buildingManager.CancelBuilding();
        SetStateGameplay();
    }

    //Toggles Ability to Place
    public void TogglePlace(int collisions) {
        if (collisions >= 1) {
            inputManager.DisablePlace();
        } else {
            inputManager.EnablePlace();
        }
    }

    //Waits for the Build Time of Buildings
    private IEnumerator WaitBuildTime() {
        yield return new WaitForSecondsRealtime(buildingManager.GetBuildTime());
    }

    public void ActivateWorkerPurchase() {
        
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

    //Gets MousePosition for Input Manager
    public Vector3 MousePosition() {
        return inputManager.GetSelectedMapPosition();
    }
    /*----------------------------------*/


    /* Nav Mesh For Unit Traversal */
    public void BakeNavMesh() {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    /*----------------------------------*/
}

