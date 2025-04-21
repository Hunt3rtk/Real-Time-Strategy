using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    //Unit Slots
    [SerializeField]
    private GameObject unitCountObject, unitSlotsObject;
    private Text unitCountText, unitSlotsText;
    //Building
    [SerializeField]
    private GameObject workerPanel, soliderPanel, juggernautPanel, wizardPanel, dragonPanel, enemyUnitPanel, buildingPanel, enemyBuildingPanel, basePanel, barracksPanel, minePanel, menuPanel;
    [SerializeField]
    private GameObject lumberObject, goldObject;
    private Text lumberText, goldText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lumberText = lumberObject.GetComponent<Text>();
        goldText = goldObject.GetComponent<Text>();

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

    internal void UpdateGold(int amount) {
        goldText.text = amount.ToString();
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
        enemyUnitPanel.SetActive(false);
        buildingPanel.SetActive(false);
        enemyBuildingPanel.SetActive(false);
        basePanel.SetActive(false);
        barracksPanel.SetActive(false);
        menuPanel.SetActive(false);
        minePanel.SetActive(false);
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
            case "EnemyUnit":
                enemyUnitPanel.SetActive(true);
                unit.name = enemyUnitPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                panel = enemyUnitPanel;
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
            case "EnemyUnit":
                enemyUnitPanel.SetActive(false);
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
        barracksPanel.GetComponent<BuildingHealthBar>().building = building;
    }
    internal void DeactivateBarracksPanel() {
        barracksPanel.SetActive(false); 
    }

    internal void ActivateBasePanel(Building building, Training training) {
        basePanel.SetActive(true);
        basePanel.GetComponent<BuildingHealthBar>().building = building;
        basePanel.GetComponent<TrainingProgressBar>().training = training;
    }

    public void ActivateBaseProgressBar(Texture headshot) {
        ActivateProgressBar(headshot, basePanel);
    }

    public void DeactivateBaseProgressBar() {
        DeactivateProgressBar(basePanel);
    }

    public void ActivateBarracksProgressBar(Texture headshot) {
        ActivateProgressBar(headshot, barracksPanel);
    }

    public void DeactivateBarracksProgressBar() {
        DeactivateProgressBar(barracksPanel);
    }

    private void ActivateProgressBar(Texture headshot, GameObject panel) {

        TrainingProgressBar trainingProgressBar = panel.GetComponent<TrainingProgressBar>();

        trainingProgressBar.unitHeadshot = headshot;

        trainingProgressBar.progressBar.transform.parent.gameObject.SetActive(true);
    }

        private void DeactivateProgressBar(GameObject panel) {
        TrainingProgressBar trainingProgressBar = panel.GetComponent<TrainingProgressBar>();

        trainingProgressBar.progressBar.transform.parent.gameObject.SetActive(false);
        trainingProgressBar.training = null;
        trainingProgressBar.unitHeadshot = null;
        trainingProgressBar.progressBar.value = 0;
        
    }

    internal void DeactivateBasePanel() {
        basePanel.SetActive(false);
    }

    internal void ActivateBuildingPanel(Building building) {
        buildingPanel.SetActive(true);
        buildingPanel.GetComponent<BuildingHealthBar>().building = building;
    }

    internal void DeactivateBuildingPanel() {
        buildingPanel.SetActive(false);
    }

    internal void ActivateEnemyBuildingPanel(Building building) {
        enemyBuildingPanel.SetActive(true);
        enemyBuildingPanel.GetComponent<BuildingHealthBar>().building = building;
    }

    internal void DeactivateEnemyBuildingPanel() {
        enemyBuildingPanel.SetActive(false);
    }

    internal void ActivateMinePanel(Mine mine) {
        minePanel.SetActive(true);
        minePanel.GetComponent<MineGoldBar>().mine = mine;
    }

    internal void DeactivateMinePanel() {
        minePanel.SetActive(false);
    }
}
