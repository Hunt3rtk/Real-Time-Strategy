using UnityEngine;

public class Worker : Unit {

    private void Start() {
        unitName = "Worker";
        health = 200f;
        speed = 3f;
        attackPower = 0f;
        defensePower = 10f;
        cooldown = 1f;
        range = 1f;
    }

    public new void Attack(Unit target) {
        return;
    }
}
