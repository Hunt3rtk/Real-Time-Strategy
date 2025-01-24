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

    public void Start() {
        bm = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        home = GameObject.Find("Base").transform;
    }

    public override void MoveStandAlone(Vector3 destination) {
        StopAllCoroutines();
        StartCoroutine(Move(destination));
    }

    public override void AttackStandAlone(Collider target) {
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

    public void StartConstruct(Vector3 destination, int id) {
        StopAllCoroutines();
        state = State.Building;
        StartCoroutine(Construct(destination, id));
    }

    public void StartRepair(Building building) {
        StopAllCoroutines();
        state = State.Working;
        StartCoroutine(Repair(building));
    }

    private IEnumerator Mine(Mine mine) {
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

    private IEnumerator Chop(Tree tree) { 
        yield return Move(tree.transform.position);
        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        animator.SetBool("isWorking", true);
        yield return new WaitForSecondsRealtime(chopTime);
        tree.ChopTree();
        animator.SetBool("isWorking", false);
        carryingLumber = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        bm.AddLumber(lumberAmount);
        carryingLumber = false;
        yield return Move(tree.transform.position);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, visibilityRange, LayerMask.GetMask("Tree"));
        colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
        if (colliders.Length <= 0) yield break;
        Tree newTree = colliders[0].gameObject.GetComponent<Tree>();
        StartChop(newTree);
    }

    private IEnumerator Construct(Vector3 destination, int id) {

        state = State.Building;

        GameObject constructionSite = bm.PlaceConstructionSite(destination, id);

        yield return Move(destination);

        animator.SetBool("isWorking", true);

        bm.PurchaseBuilding(id);

        yield return new WaitForSecondsRealtime(bm.GetBuildTime(id));

        yield return bm.PlaceBuilding(destination, id);

        Destroy(constructionSite, 0);

        yield return new WaitForSecondsRealtime(.5f);

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));

        animator.SetBool("isWorking", false);
    }

    private IEnumerator Repair(Building building) {
        state = State.Working;
        animator.SetBool("isWorking", true);
        yield return Move(building.transform.position);
        while(building.Health < building.maxHealth) {
            yield return new WaitForSecondsRealtime(1.5f);
            building.Health += 100;
        }
        building.Repaired();
        animator.SetBool("isWorking ", false);
        state = State.Idle;
        gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(.5f);

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));

        gameObject.SetActive(true);
    }
}
