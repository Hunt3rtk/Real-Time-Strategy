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
    public int goldAmount = 100;      // Changed from metalAmount
    public int lumberAmount = 100;

    private  SoundType chopSound;
    private  SoundType constructSound;

    public Transform home;
    public BuildingManager bm;

    public bool carryingLumber = false;
    public bool carryingGold = false; // Changed from carryingMetal

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
        if(mine.Gold < goldAmount) yield break;
        mine.Deduct(goldAmount);
        carryingGold = true;
        yield return Move(home.position);
        while (agent.remainingDistance >  agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        carryingGold = false;
        bm.AddGold(goldAmount);       // Changed from AddMetal
        StartMine(mine);
    }

    private IEnumerator Chop(Tree tree) { 
        yield return Move(tree.transform.position);
        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }
        animationPlayer.PlayWork();
        StartCoroutine(AudioManager.Instance.Play(chopSound));
        yield return new WaitForSecondsRealtime(chopTime);
        tree.ChopTree();
        animationPlayer.StopWork();
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

    private void Construct(Vector3 destination, int id) {

        state = State.Building;

        //GameObject constructionSite = bm.PlaceConstructionSite(destination, id);

        bm.PurchaseBuilding(id);

        Building building = bm.PlaceBuilding(destination, id).transform.GetChild(0).GetComponent<Building>();
        if (building == null) return;
        building.Health = 1;
        building.gameObject.transform.parent.transform.GetChild(0).gameObject.SetActive(false);
        building.gameObject.transform.parent.transform.GetChild(2).gameObject.SetActive(true);

        //yield return Move(destination);

        StartRepair(building);

        //animator.SetBool("isWorking", true);

        //bm.PurchaseBuilding(id);

        //yield return new WaitForSecondsRealtime(bm.GetBuildTime(id));

        //yield return bm.PlaceBuilding(destination, id);

        //Destroy(constructionSite, 0);

        //yield return new WaitForSecondsRealtime(.5f);

        // NavMeshHit hit;
        // NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        // agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));

        // animator.SetBool("isWorking", false);
    }

    private IEnumerator Repair(Building building) {
        state = State.Working;

        yield return Move(building.transform.position);

        animationPlayer.PlayWork();

        while(building.Health < building.maxHealth) {
            StartCoroutine(AudioManager.Instance.Play(constructSound));
            yield return new WaitForSecondsRealtime(1.5f);
            building.Health += 100;
        }

        building.Repaired();

        animationPlayer.StopWork();
        state = State.Idle;

        yield return new WaitForSecondsRealtime(.5f);

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);
        agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));
    }
}
