using UnityEngine;

public class Mine : MonoBehaviour {

    public int metalRemaining = 60000;

    public void Deduct(int amount) {
        metalRemaining -= amount;
        if (metalRemaining <= 0) this.gameObject.SetActive(false);
    }
}
