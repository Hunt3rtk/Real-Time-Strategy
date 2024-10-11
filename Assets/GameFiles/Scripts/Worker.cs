using System.Buffers.Text;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
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
        chopTime = 2f;
        mineTime = 2f;
        carryingLumber = false;
        carryingMetal = false;
    }

    public new void Attack(Unit target) {
        return;
    }

    public IEnumerator Mine(Vector3 destination) {
        yield return Move(destination);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        state = State.Mining;
        yield return new WaitForSecondsRealtime(chopTime);
        carryingMetal = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        carryingMetal = false;
        yield return Mine(destination);
    }

    public IEnumerator Chop(Vector3 destination) {
        Collider[] colliders;
        yield return Move(destination);
        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        colliders = Physics.OverlapSphere(this.transform.position, 7, LayerMask.GetMask("Tree"));
        if (colliders.Length <= 0) yield break;
        yield return Move(colliders[0].transform.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        state = State.Chopping;
        yield return new WaitForSecondsRealtime(chopTime);
        carryingLumber = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        carryingLumber = false;
        yield return Chop(destination);
    }

    public IEnumerator Construct(Vector3 destination) {
        yield return Move(destination);
        state = State.Building;
        gameObject.SetActive(false);
    }
}
