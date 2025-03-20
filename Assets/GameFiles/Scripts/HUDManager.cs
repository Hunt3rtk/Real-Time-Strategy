using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Unit Slots
    [SerializeField]
    private GameObject unitCountObject, unitSlotsObject;
     private Text unitCountText, unitSlotsText;
    //Building
    [SerializeField]
    private GameObject workerPanel, soliderPanel, juggernautPanel, wizardPanel, dragonPanel, buildingPanel, basePanel, barracksPanel, menuPanel;
    [SerializeField]
    private GameObject lumberObject, metalObject;
    private Text lumberText, metalText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lumberText = lumberObject.GetComponent<Text>();
        metalText = metalObject.GetComponent<Text>();

        unitCountText = unitCountObject.GetComponent<Text>();
        unitSlotsText = unitSlotsObject.GetComponent<Text>();
    }
    internal GameObject GetWorkerPanel() {
        return workerPanel;
    }

    internal GameObject GetBasePanel() {
        return basePanel;
    }

    internal void UpdateLumber(int amount) {
        lumberText.text = amount.ToString();
    }

    internal void UpdateMetal(int amount) {
        metalText.text = amount.ToString();
    }

     internal void UpdateUnitCount(int amount) {
        unitCountText.text = amount.ToString();
    }

    internal void UpdateUnitSlots(int amount) {
        unitSlotsText.text = amount.ToString();
    }

    internal void DeactivateAllPanels() {
        workerPanel.SetActive(false);
        soliderPanel.SetActive(false);
        juggernautPanel.SetActive(false);
        wizardPanel.SetActive(false);
        dragonPanel.SetActive(false);
        basePanel.SetActive(false);
        barracksPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    internal void ActivateUnitPanel(Unit unit) {
        GameObject panel = null;
        switch(unit.tag) {
            case "Worker":
                workerPanel.SetActive(true);
                panel = workerPanel;
                break;
            case "Solider":
                soliderPanel.SetActive(true);
                panel = soliderPanel;
                break;
            case "Juggernaut":
                juggernautPanel.SetActive(true);
                panel = juggernautPanel;
                break;
            case "Wizard":
                wizardPanel.SetActive(true);
                panel = wizardPanel;
                break;
            case "Dragon":
                dragonPanel.SetActive(true);
                panel = dragonPanel;
                break;
            default:
                break;
        }
        panel.GetComponent<HealthBar>().unit = unit;
    }

    internal void DeactivateUnitPanel(Unit unit) {
        switch(unit.tag) {
            case "Worker":
                workerPanel.SetActive(false);
                break;
            case "Solider":
                soliderPanel.SetActive(false);
                break;
            case "Juggernaut":
                juggernautPanel.SetActive(false);
                break;
            case "Wizard":
                wizardPanel.SetActive(false);
                break;
            case "Dragon":
                dragonPanel.SetActive(false);
                break;
            default:
                break;

        }
    }

    internal void ActivateMenuPanel() {
        menuPanel.SetActive(true);
    }

    internal void DeactivateMenuPanel() {
        menuPanel.SetActive(false);
    }

    internal void ActivateBarracksPanel(Building building) {
        barracksPanel.SetActive(true);
        GetComponent<BuildingHealthBar>().building = building;
    }
    internal void DeactivateBarracksPanel() {
        barracksPanel.SetActive(false); 
    }

    internal void ActivateBasePanel(Building building) {
        basePanel.SetActive(true);
        GetComponent<BuildingHealthBar>().building = building;
    }

    internal void DeactivateBasePanel() {
        basePanel.SetActive(false);
    }

    internal void ActivateBuildingPanel(Building building) {
        buildingPanel.SetActive(true);
        GetComponent<BuildingHealthBar>().building = building;
    }

    internal void DeactivateBuildingPanel() {
        buildingPanel.SetActive(false);
    }
}
