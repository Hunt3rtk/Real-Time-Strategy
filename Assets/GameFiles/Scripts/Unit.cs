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
    public Animator animator;

    [SerializeField]
    internal State state;

    [SerializeField]
    internal Guard guard;

    internal SoundType selectSound;
    internal SoundType commandSound;
    internal SoundType spawnSound;
    internal SoundType attackSound;
    internal SoundType deathSound;

    [HideInInspector]
    public NavMeshAgent agent;


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
        animator = GetComponent<Animator>();
        Health = maxHealth;
    }

    public virtual void MoveStandAlone(Vector3 destination) {
        StopAllCoroutines();
        StartCoroutine(Move(destination));
    }

    public virtual void AttackStandAlone(Collider target) {
        StopAllCoroutines();
        StartCoroutine(Attack(target));
    }

    public IEnumerator Move(Vector3 destination) {

        // Movement logic
        agent.SetDestination(PositionNormailze(destination));
        while (agent.pathPending) {
            yield return new WaitForSecondsRealtime(.1f);
        }

        state = State.Moving;
        animator.SetBool("isWalking", true);

        yield return null;

        while (agent.remainingDistance > agent.stoppingDistance) {
            yield return new WaitForSecondsRealtime(.2f);
        }

        state = State.Idle;
        animator.SetBool("isWalking", false);

        try {
            StartCoroutine(guard.CheckVisibility(visibilityRange));
        } catch {
            Debug.Log("Guard is null");
        }
        
    }

    public IEnumerator Attack(Collider target) {
        state = State.Attacking;
        Unit targetUnit = target.GetComponent<Unit>();
        Building targetBuilding;

        try {
            targetBuilding = target.GetComponent<Building>();
        } catch {
            targetBuilding = null;
        }

        if (targetBuilding == null) {
            while (targetUnit.Health > 0) {

                agent.SetDestination(PositionNormailze(target.transform.position));
                yield return null;

                if (agent.remainingDistance <= range) {
                    agent.SetDestination(agent.transform.position);
                    animator.SetBool("isAttacking", true);
                    AudioManager.Instance.Play(attackSound);
                    targetUnit.Health -= damage;
                    yield return new WaitForSecondsRealtime(cooldown);
                } else {
                    yield return new WaitForSecondsRealtime(.2f);
                }
            }
        } else {
            while (targetBuilding.Health > 0) {

                agent.SetDestination(PositionNormailze(target.transform.position));
                yield return null;

                if (agent.remainingDistance <= range) {
                    agent.SetDestination(agent.transform.position);
                    animator.SetBool("isAttacking", true);
                    targetBuilding.Health -= damage;
                    yield return new WaitForSecondsRealtime(cooldown);
                } else {
                    yield return new WaitForSecondsRealtime(.2f); 
                }
            }
        }

        state = State.Idle;
        animator.SetBool("isAttacking", false);

        try {
            StartCoroutine(guard.CheckVisibility(visibilityRange));
        } catch {
            Debug.Log("Guard is null");
        }
    }

    public Vector3 PositionNormailze(Vector3 destination) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 10f, NavMesh.AllAreas)) {
            return hit.position;
        }
        return Vector3.zero;
    }

    private void Kill() {
        AudioManager.Instance.Play(deathSound);
        animator.SetBool("isDead", true);
        Destroy(this.gameObject);
    }
}