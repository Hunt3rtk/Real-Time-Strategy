using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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
            if (health <= 0) Kill();
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

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        guard = GetComponent<Guard>();
        animationPlayer = GetComponent<UnitAnimationPlayer>();
        damageFlash = GetComponent<DamageFlash>();
        Health = maxHealth;
        SetStateIdle();
        if (guard != null) {
            guard.StartSearch();
        }
    }

    public virtual void SetStateIdle()
    {
        state = State.Idle;
        agent.isStopped = true;
        agent.ResetPath();
        animationPlayer.PlayIdle();
    }

    public virtual void SetStateMoving() {
        state = State.Moving;
        agent.isStopped = false;
        animationPlayer.PlayWalk();
    }

    public virtual void SetStateDead() {
        state = State.Dead;
        StopAllCoroutines();
        Destroy(GetComponent<Guard>());
        Destroy(GetComponent<Enemy>());
        agent.isStopped = true;
        animationPlayer.PlayDeath();
    }

    public virtual void SetStateAttack( Unit targetUnit, Building targetBuilding = null) {
        this.transform.LookAt((targetBuilding != null) ? targetBuilding.transform.position : targetUnit.transform.position);
        animationPlayer.PlayAttack();
        StartCoroutine(AudioManager.Instance.Play(attackSound));
        if (targetBuilding != null) {
            targetBuilding.strikeDelay = AudioManager.Instance.GetStrikeDelay(attackSound);
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

        StopAllCoroutines();
        state = State.Attacking;
        StartCoroutine(Attack(target));
    }

    public IEnumerator Move(Vector3 destination)
    {
        // Movement logic
        agent.SetDestination(NearestDestination(destination));
        while (agent.pathPending)
        {
            yield return new WaitForSecondsRealtime(.1f);
        }

        SetStateMoving();

        yield return null;

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return new WaitForSecondsRealtime(.2f);
        }

        SetStateIdle();
        
        if (guard != null) {
            guard.StartSearch();
        }
    }

    public IEnumerator Attack(Collider target)
    {

        Unit targetUnit = target.GetComponent<Unit>();
        Building targetBuilding = target.GetComponent<Building>();

        if (targetUnit != null)
        {
            while (targetUnit.Health > 0)
            {

                yield return agent.SetDestination(DestinationCalculation(target));

                while (agent.pathPending)
                {
                    yield return new WaitForSecondsRealtime(.1f);
                }

                SetStateMoving();

                if (agent.remainingDistance <= range)
                {

                    SetStateIdle();

                    SetStateAttack(targetUnit);

                    yield return CooldownTimer(cooldown);
                }

                yield return new WaitForSeconds(.2f);
            }
        }
        else if (targetBuilding != null)
        {
            while (targetBuilding.Health > 0)
            {

                yield return agent.SetDestination(DestinationCalculation(target));

                while (agent.pathPending)
                {
                    yield return new WaitForSecondsRealtime(.1f);
                }

                SetStateMoving();

                if (agent.remainingDistance <= range)
                {

                    SetStateIdle();

                    SetStateAttack(null, targetBuilding);

                    yield return CooldownTimer(cooldown);
                }
                yield return new WaitForSecondsRealtime(.2f);
            }
        }

        SetStateIdle();

        if (guard != null)
        {
            guard.StartSearch();
        }
        yield return null;
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

    private void Kill() {

        StartCoroutine(AudioManager.Instance.Play(deathSound));
        
        SetStateDead();

        if (gameObject.tag == "Unit")
        {
            int unitCount = FindAnyObjectByType<GameManager>().unitCount -= 1;
            FindAnyObjectByType<HUDManager>().UpdateUnitCount(unitCount);
            GameManager.Instance.RemoveSelectedUnit(this);
        }

        this.gameObject.tag = "Dead";
        this.gameObject.layer = LayerMask.NameToLayer("DeadUnit");

        Destroy(this.gameObject, 20f);
    }
}