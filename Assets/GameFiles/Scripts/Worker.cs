using UnityEngine;

public class Worker : Unit {

    public float mineCooldown;
    public float chopCooldown;
    public float buildCooldown;


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

    public void Work(Vector3 destination) {
        Move(destination);
        while(state == State.Moving) {}
        state = State.Working;
    }

    public void Mine(Vector3 destination) {
        Work(destination);

    }

    public void Chop(Vector3 destination) {
        Work(destination);
    }

    public void Construct(Vector3 destination) {
        Work(destination);
    }
}
