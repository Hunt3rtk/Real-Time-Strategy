using System.Collections;
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
        Working,
        Building,
        Chopping,
        Mining,
        Dead
    }

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {

        switch(state) {
            case State.Moving:
                if (agent.isStopped) state = State.Idle;
                break;
            case State.Attacking:
                break;
            case State.Working:
                break;
            case State.Idle:
                break;
            case State.Dead:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public void Move(Vector3 destination) {
        // Movement logic
        agent.SetDestination(destination);
        state = State.Moving;
    }

    public IEnumerator Attack(Unit target) {
        this.target = target;
        agent.SetDestination(target.transform.position);
        state = State.Attacking;
        while (this.target.health > 0) {
            if (agent.remainingDistance < range) {
                    yield return target.health -= attackPower;
            } else {
                    yield return agent.destination = target.transform.position;
                }
            yield return new WaitForSecondsRealtime(cooldown);
        }
        target.state = State.Dead;
        state = State.Idle;
    }
}