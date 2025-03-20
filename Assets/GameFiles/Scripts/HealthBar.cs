using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    public Unit unit;

    [SerializeField]
    private Slider healthBar;

    void Update() {
        if (this.gameObject.activeSelf) {

            //Update Health Bar
            healthBar.value = (float)unit.Health / (float)unit.maxHealth;
            
        }
    }
}
