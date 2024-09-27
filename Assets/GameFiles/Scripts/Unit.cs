using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public float health;
    public float speed;
    public float cooldown;
    private float cooldownTimer;
    public float attackPower;
    public float defensePower;
    public float range;
    public State state;

    [HideInInspector]
    public NavMeshAgent agent;

    private Unit target; 

    public enum State {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        cooldownTimer = cooldown;
    }

    void Update() {

        cooldownTimer -= Time.deltaTime;

        switch(state) {
            case State.Moving:
                agent.isStopped = false;
                if (agent.remainingDistance <= agent.stoppingDistance) state = State.Idle;
                break;
            case State.Attacking:
                if (cooldownTimer > 0) {
                    break;
                }

                if (target.health <= 0) {
                    target.state = State.Dead;
                    agent.isStopped = true;
                    state = State.Idle;
                    break;
                }
                if (agent.remainingDistance < range) {
                    agent.isStopped = true;
                    target.health -= attackPower;
                    cooldownTimer = cooldown;
                } else {
                    agent.isStopped = false;
                    agent.destination = target.transform.position;
                }
                break;
            case State.Idle:
                break;
            case State.Dead:
            default:
                break;
        }
    }

    public void Move(Vector3 destination) {
        // Movement logic
        agent.SetDestination(destination);
        state = State.Moving;
    }

    public void Attack(Unit target) {
        this.target = target;
        agent.SetDestination(target.transform.position);
        state = State.Attacking;
    }
}