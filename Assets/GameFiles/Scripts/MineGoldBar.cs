using UnityEngine;
using UnityEngine.UI;

public class MineGoldBar : MonoBehaviour {

    public Mine mine;

    [SerializeField]
    private Slider goldBar;

    void Update() {
        if (this.gameObject.activeSelf && mine != null) {

            // Update Gold Bar to show remaining percentage
            goldBar.value = (float)mine.Gold / (float)mine.maxGold;
        }
    }
}