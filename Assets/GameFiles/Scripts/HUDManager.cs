using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    //Building
    [SerializeField]
    private GameObject buildingPanel, basePanel;
    [SerializeField]
    private GameObject lumberObject, metalObject;
    private TextMeshProUGUI lumberText, metalText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lumberText = lumberObject.GetComponent<TextMeshProUGUI>();
        metalText = metalObject.GetComponent<TextMeshProUGUI>();
    }
    internal GameObject GetBuildingPanel() {
        return buildingPanel;
    }

    internal GameObject GetBasePanel() {
        return basePanel;
    }

    internal void UpdateLumber(int amount) {
        lumberText.SetText(amount.ToString());
    }

    internal void UpdateMetal(int amount) {
        metalText.SetText(amount.ToString());
    }

    internal void ActivateUnitPanel(Unit unit) {
        switch(unit.name) {
            case "Worker":
                buildingPanel.SetActive(true);
                break;
            default:
                break;

        }
        
    }

    internal void DeactivateUnitPanel(Unit unit) {
        switch(unit.name) {
            case "Worker":
                buildingPanel.SetActive(false);
                break;
            default:
                break;

        }
    }

    internal void ActivateBasePanel() {
        basePanel.SetActive(true);
    }

    internal void DeactivateBasePanel() {
        basePanel.SetActive(false);
    }
}
