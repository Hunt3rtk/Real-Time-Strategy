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
    private GameObject buildingPanel, basePanel, barracksPanel;
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
    internal GameObject GetBuildingPanel() {
        return buildingPanel;
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
        buildingPanel.SetActive(false);
        basePanel.SetActive(false);
        barracksPanel.SetActive(false);
    }

    internal void ActivateUnitPanel(Unit unit) {
        switch(unit.tag) {
            case "Worker":
                buildingPanel.SetActive(true);
                break;
            default:
                break;

        }
        
    }

    internal void DeactivateUnitPanel(Unit unit) {
        switch(unit.tag) {
            case "Worker":
                buildingPanel.SetActive(false);
                break;
            default:
                break;

        }
    }

    internal void ActivateBarracksPanel() {
        barracksPanel.SetActive(true);
    }
    internal void DeactivateBarracksPanel() {
        barracksPanel.SetActive(false); 
    }

    internal void ActivateBasePanel() {
        basePanel.SetActive(true);
    }

    internal void DeactivateBasePanel() {
        basePanel.SetActive(false);
    }
}
