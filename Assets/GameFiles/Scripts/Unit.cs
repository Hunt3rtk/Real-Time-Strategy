using System.Collections;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static AudioManager;

public class Unit : MonoBehaviour {
    public string unitName;

    [SerializeField]
    private float health;
    public float Health {
        get {
            return health;
        }

        set {
            if (value < health && damageFlash != null) {
                StartCoroutine(damageFlash.Flash(strikeDelay));
            }
            health = value;
            if (health > maxHealth) health = maxHealth;
            if (health <= 0) StartCoroutine(Kill());
        }
    }
    public float maxHealth;
    public float speed;
    public float cooldown;
    public float damage;
    public float range;
    public float visibilityRange;
    internal UnitAnimationPlayer animationPlayer;

    [SerializeField]
    internal State state;

    private Guard guard;

    internal SoundType selectSound;
    internal SoundType commandSound;
    internal SoundType spawnSound;
    internal SoundType attackSound;
    internal SoundType deathSound;

    [HideInInspector]
    public NavMeshAgent agent;

    private bool iscooldown = false;

    private DamageFlash damageFlash;

    private float strikeDelay = 0f;


    public enum State {
        Idle,
        Moving,
        Attacking,
        Working,
        Building,
        Chopping,
        Mining,
        Dead
    }

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        guard = GetComponent<Guard>();
        animationPlayer = GetComponent<UnitAnimationPlayer>();
        damageFlash = GetComponent<DamageFlash>();
        Health = maxHealth;
    }

    public virtual void SetStateIdle() {
        //state = State.Idle;
        agent.isStopped = true;
        agent.ResetPath();
        animationPlayer.PlayIdle();
    }

    public virtual void SetStateMoving() {
        //state = State.Moving;
        agent.isStopped = false;
        animationPlayer.PlayWalk();
    }

    public virtual void SetStateDead() {
        //state = State.Dead;
        agent.isStopped = true;
        animationPlayer.PlayDeath();
    }

    public virtual void SetStateAttack( Unit targetUnit, Building targetBuilding = null) {
        this.transform.LookAt((targetBuilding != null) ? targetBuilding.transform.position : targetUnit.transform.position);
        animationPlayer.PlayAttack();
        StartCoroutine(AudioManager.Instance.Play(attackSound));
        if (targetBuilding != null) {
            targetBuilding.Health -= damage;
        } else {
            targetUnit.strikeDelay = AudioManager.Instance.GetStrikeDelay(attackSound);
            targetUnit.Health -= damage;
        }
    }
    

    public virtual void MoveStandAlone(Vector3 destination) {

        StopAllCoroutines();
        state = State.Moving;
        StartCoroutine(Move(destination));
    }

    public virtual void AttackStandAlone(Collider target) {

        if (iscooldown) return;

        state = State.Attacking;
        StopAllCoroutines();
        StartCoroutine(Attack(target));
    }

    public IEnumerator Move(Vector3 destination) {

        // Movement logic
        agent.SetDestination(NearestDestination(destination));
        while (agent.pathPending) {
            yield return new WaitForSecondsRealtime(.1f);
        }

        SetStateMoving();

        yield return null;

        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }

        SetStateIdle();

        try {
            StartCoroutine(guard.CheckVisibility(visibilityRange));
        } catch {
            Debug.Log("Guard is null");
        }
        
    }

    public IEnumerator Attack(Collider target) {
        Unit targetUnit = target.GetComponent<Unit>();
        Building targetBuilding = target.GetComponent<Building>();

        if (targetBuilding == null) {
            while (targetUnit.Health > 0) {

                yield return agent.SetDestination(DestinationCalculation(target));

                while (agent.pathPending) {
                     yield return new WaitForSecondsRealtime(.1f);
                }
                
                SetStateMoving();

                if (agent.remainingDistance <= range) {
                    
                    SetStateIdle();

                    SetStateAttack(targetUnit);

                    yield return CooldownTimer(cooldown);
                }

                yield return new WaitForSeconds(.2f);
            }
        } else {
            while (targetBuilding.Health > 0) {

                yield return agent.SetDestination(DestinationCalculation(target));

                while (agent.pathPending) {
                    yield return new WaitForSecondsRealtime(.1f);
                }

                SetStateMoving();

                while (agent.remainingDistance <= range) {

                    SetStateIdle();
                    
                    SetStateAttack(null, targetBuilding);

                    yield return CooldownTimer(cooldown);
                }

                SetStateMoving();
                yield return new WaitForSecondsRealtime(.2f);
            }
        }

        SetStateIdle();
    }

    public Vector3 DestinationCalculation(Collider target) {
        return NearestDestination(target.ClosestPoint(this.transform.position));
    }

    public Vector3 NearestDestination(Vector3 destination) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 50f, NavMesh.AllAreas)) {
            return hit.position;
        }
        return Vector3.one * 1000f;
    }

    public IEnumerator CooldownTimer(float cooldown) {
        iscooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        iscooldown = false;
    }

    private IEnumerator Kill() {
        StartCoroutine(AudioManager.Instance.Play(deathSound));
        //TODO death animation
        SetStateDead();

        if(gameObject.tag == "Unit") {
            int unitCount = FindAnyObjectByType<GameManager>().unitCount -= 1;
            FindAnyObjectByType<HUDManager>().UpdateUnitCount(unitCount);
        }
        this.gameObject.tag = "Dead";
        //yield return new WaitForSecondsRealtime(30f);
        Destroy(this.gameObject);
        yield return null;
    }
}