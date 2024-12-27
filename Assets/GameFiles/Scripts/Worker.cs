using System.Buffers.Text;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Worker : Unit {
    public float mineTime = 2f;
    public float chopTime = 2f;
    public int metalAmount = 100;
    public int lumberAmount = 100;
    public Transform home;
    public BuildingManager bm;
    public bool carryingLumber = false;
    public bool carryingMetal = false;

    public new void Attack(Collider target) {
        return;
    }

    public void StartMine(Mine mine) {
        StopAllCoroutines();
        state = State.Mining;
        StartCoroutine(Mine(mine));
    }
    public void StartChop(Tree tree) {
        StopAllCoroutines();
        state = State.Chopping;
        StartCoroutine(Chop(tree));
    }

    public IEnumerator Mine(Mine mine) {
        yield return Move(mine.transform.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        yield return new WaitForSecondsRealtime(mineTime);
        if(mine.metalRemaining < metalAmount) yield break;
        mine.Deduct(metalAmount);
        carryingMetal = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        carryingMetal = false;
        bm.AddMetal(metalAmount);
        StartMine(mine);
    }

    public IEnumerator Chop(Tree tree) { 
        yield return Move(tree.transform.position);
        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        yield return new WaitForSecondsRealtime(chopTime);
        tree.ChopTree();
        carryingLumber = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        bm.AddLumber(lumberAmount);
        carryingLumber = false;
        yield return Move(tree.transform.position);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, visibilityRange, LayerMask.GetMask("Tree"));
        if (colliders.Length <= 0) yield break;
        Tree newTree = colliders[0].gameObject.GetComponent<Tree>();
        StartChop(newTree);
    }

    public IEnumerator Construct(Vector3 destination) {
        state = State.Building;
        yield return Move(destination);
        state = State.Idle;
        gameObject.SetActive(false);
    }

    public IEnumerator Repair(Building building) {
        state = State.Working;
        yield return Move(building.transform.position);
        while(building.Health < building.maxHealth) {
            yield return new WaitForSecondsRealtime(1.5f);
            building.Health += 100;
        }
        building.Repaired();
        state = State.Idle;
        gameObject.SetActive(false);
    }
}
