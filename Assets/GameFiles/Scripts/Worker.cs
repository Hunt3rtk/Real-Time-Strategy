using System.Buffers.Text;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static AudioManager;

public class Worker : Unit {
    public float mineTime = 2f;
    public float chopTime = 2f;
    public int goldAmount = 100;
    public int lumberAmount = 100;

    private  SoundType chopSound;
    private  SoundType constructSound;

    public Transform home;
    public BuildingManager bm;

    public bool carryingLumber = false;
    public bool carryingGold = false;

    public void Start() {
        bm = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        home = GameObject.Find("Base").transform;

        selectSound = AudioManager.SoundType.Select;
        commandSound = AudioManager.SoundType.Command;
        spawnSound = AudioManager.SoundType.WorkerSpawn;
        chopSound = AudioManager.SoundType.WorkerChop;
        constructSound = AudioManager.SoundType.WorkerConstruct;
        deathSound = AudioManager.SoundType.WorkerDeath;
    }

    private void SetStateWorking() {
        state = State.Working;
        agent.isStopped = true;
        agent.ResetPath();
        animationPlayer.PlayWork();
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
        Construct(destination, id);
    }

    public void StartRepair(Building building) {

        if (building == null || carryingLumber || carryingGold) {
            StartCoroutine(ReturnResources());
            return;
        }

        StopAllCoroutines();
        state = State.Working;
        StartCoroutine(Repair(building));
    }

    private IEnumerator Mine(Mine mine) {

        if (carryingLumber || carryingGold) {
            yield return ReturnResources();
        }

        yield return Move(mine.transform.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }

        SetStateWorking();

        yield return new WaitForSecondsRealtime(mineTime);
        if (mine.Gold < goldAmount) mine.Deduct(mine.Gold);
        else mine.Deduct(goldAmount);

        carryingGold = true;
        animationPlayer.animator.SetBool("isCarryingGold", true);

        yield return ReturnResources();

        StartMine(mine);
    }

    private IEnumerator Chop(Tree tree) { 

        if (carryingLumber || carryingGold) {
            yield return ReturnResources();
        }

        yield return Move(tree.transform.position);
        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }

        SetStateWorking();

        StartCoroutine(AudioManager.Instance.Play(chopSound));
        yield return new WaitForSecondsRealtime(chopTime);
        tree.ChopTree();

        SetStateIdle();

        carryingLumber = true;
        animationPlayer.animator.SetBool("isCarryingLumber", true);
        StartCoroutine(AudioManager.Instance.Play(chopSound));

        yield return ReturnResources();

        yield return Move(tree.transform.position);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, visibilityRange, LayerMask.GetMask("Tree"));
        colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
        if (colliders.Length <= 0) yield break;
        Tree newTree = colliders[0].gameObject.GetComponent<Tree>();
        StartChop(newTree);
    }

    private void Construct(Vector3 destination, int id) {

        bm.PurchaseBuilding(id);

        Building building = bm.PlaceBuilding(destination, id).transform.GetChild(0).GetComponent<Building>();
        if (building == null) return;
        building.Health = 1;
        building.gameObject.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
        building.gameObject.transform.parent.transform.GetChild(2).gameObject.SetActive(true);

        StartRepair(building);
    }

    private IEnumerator Repair(Building building) {

        if (carryingGold || carryingLumber) {
            yield return ReturnResources();
        }

        yield return Move(building.transform.position);

        SetStateWorking();

        while(building.Health < building.maxHealth) {
            StartCoroutine(AudioManager.Instance.Play(constructSound));
            yield return new WaitForSecondsRealtime(1.5f);
            building.Health += 100;
        }

        building.Health = building.maxHealth;

        building.Repaired();

        SetStateIdle();

        yield return new WaitForSecondsRealtime(.5f);

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));
    }

    private IEnumerator ReturnResources() {

        yield return Move(home.position);

        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }

        if (carryingGold) {

            carryingGold = false;
            animationPlayer.animator.SetBool("isCarryingGold", false);
            bm.AddGold(goldAmount);

        } else if (carryingLumber) {

            carryingLumber = false;
            animationPlayer.animator.SetBool("isCarryingLumber", false);
            bm.AddLumber(lumberAmount);

        }

        yield return null;
    }
}
