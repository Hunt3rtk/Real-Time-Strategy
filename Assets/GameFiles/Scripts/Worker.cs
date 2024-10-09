using System.Buffers.Text;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Worker : Unit {

    public float mineTime;
    public float chopTime;
    public Transform home;
    public bool carryingLumber;
    public bool carryingMetal;

    private void Start() {
        unitName = "Worker";
        health = 200f;
        speed = 3f;
        attackPower = 0f;
        defensePower = 10f;
        cooldown = 1f;
        range = 1f;
        carryingLumber = false;
        carryingMetal = false;
    }

    public new void Attack(Unit target) {
        return;
    }

    public IEnumerator Work(Vector3 destination) {
        yield return Move(destination);
        while (state == State.Moving) {
            yield return new WaitForSeconds(.2f);
        }
    }

    public IEnumerator Mine(Vector3 destination) {
        destination.z -= 4.50f;
        yield return Work(destination);
        state = State.Mining;
        yield return new WaitForSecondsRealtime(chopTime);
        carryingMetal = true;
        yield return Move(PositionNormalize(home.position));
        carryingMetal = false;
    }

    public IEnumerator Chop(Vector3 destination) {
        destination.z -= 4.50f;
        yield return Work(destination);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 7, LayerMask.GetMask("Tree"));
        if (colliders.Length <= 0) yield break;
        yield return Work(PositionNormalize(colliders[0].transform.position));
        state = State.Chopping;
        yield return new WaitForSecondsRealtime(chopTime);
        carryingLumber = true;
        yield return Work(PositionNormalize(home.position));
        carryingLumber = false;
    }

    public IEnumerator Construct(Vector3 destination) {
        destination.z -= 4.50f;
        yield return Work(destination);
        state = State.Building;
        gameObject.SetActive(false);
    }
}
