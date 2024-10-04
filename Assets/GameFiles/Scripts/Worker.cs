using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    public IEnumerator Work(Vector3 destination) {
        Move(destination);
        while (state == State.Moving) {
            yield return new WaitForSeconds(.2f);
        }
    }

    public IEnumerator Mine(Vector3 destination) {
        yield return Work(destination);
    }

    public IEnumerator Chop(Vector3 destination) {
        yield return Work(destination);
    }

    public IEnumerator Construct(Vector3 destination) {
        destination.z = destination.z - 4.50f;
        yield return Work(destination);
        state = State.Building;
        gameObject.SetActive(false);
    }
}
