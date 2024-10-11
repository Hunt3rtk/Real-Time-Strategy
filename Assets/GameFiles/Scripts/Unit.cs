using System.Collections;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    }

    public IEnumerator Move(Vector3 destination) {
        // Movement logic
        agent.SetDestination(PositionNormailze(destination));
        yield return null;
        while (agent.remainingDistance > agent.stoppingDistance) {
            state = State.Moving;
            yield return new WaitForSecondsRealtime(.2f);
        }
        state = State.Idle;
    }

    public IEnumerator Attack(Unit target) {
        this.target = target;
        agent.SetDestination(PositionNormailze(target.transform.position));
        state = State.Attacking;
        while (this.target.health > 0) {
            if (agent.remainingDistance < range) {
                    yield return target.health -= attackPower;
            } else {
                    yield return agent.destination = target.transform.position;
                }
            yield return new WaitForSecondsRealtime(cooldown);
        }
        target.Kill();
        state = State.Idle;
    }

    public Vector3 PositionNormailze(Vector3 destination) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 10f, NavMesh.AllAreas)) {
            return hit.position;
        }
        return Vector3.zero;
    }

    public void Kill() {
        Destroy(this);
    }
}