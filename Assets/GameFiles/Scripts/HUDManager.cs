using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    //Game Manager
    [SerializeField]
    private GameManager gm;

    //Building
    [SerializeField]
    private GameObject buildingPanel, basePanel;
    [SerializeField]
    private GameObject lumberObject, metalObject;
    private TextMeshPro lumberText, metalText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lumberText = lumberObject.GetComponent<TextMeshPro>();
        metalText = metalObject.GetComponent<TextMeshPro>();
    }
    internal GameObject GetBuildingPanel() {
        return buildingPanel;
    }

    internal GameObject GetBasePanel() {
        return basePanel;
    }

    internal void UpdateLumber(string amount) {
        lumberText.SetText(amount);
    }

    internal void UpdateMetal(string amount) {
        metalText.SetText(amount);
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
}
