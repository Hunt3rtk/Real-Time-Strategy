using UnityEngine;

public class EnemyBuilding : Building {

    public override void Repaired() {
        return;
    }

    public override void Kill() {
        gameObject.SetActive(false);
    }
}
