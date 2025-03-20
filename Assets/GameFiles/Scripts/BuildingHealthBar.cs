using UnityEngine;
using UnityEngine.UI;

public class BuildingHealthBar : MonoBehaviour {

    public Building building;

    [SerializeField]
    private Slider healthBar;

    void Update() {
        if (this.gameObject.activeSelf) {

            //Update Health Bar
            healthBar.value = (float)building.Health / (float)building.maxHealth;
            
        }
    }
}
