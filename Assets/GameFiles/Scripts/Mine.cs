using UnityEngine;

public class Mine : MonoBehaviour {

    public int gold;
    public int maxGold;

    public int Gold {
        get {
            return gold;
        }

        set {
            gold = value;
            if (gold > maxGold) gold = maxGold;
            if (gold <= 0) Kill();
        }
    }

    void Start() {
        Gold = maxGold;
    }

    public void Deduct(int amount) {
        Gold -= amount;
    }

    public virtual void Kill() {
        this.gameObject.SetActive(false);
    }
}
