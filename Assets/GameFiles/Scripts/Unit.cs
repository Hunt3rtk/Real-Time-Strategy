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
    }

    public IEnumerator Move(Vector3 destination) {
        // Movement logic
        agent.destination = destination;
        while (agent.remainingDistance > 0) {
            state = State.Moving;
            yield return null;
        }
        state = State.Idle;
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
        target.Kill();
        state = State.Idle;
    }

    public Vector3 PositionNormalize(Vector3 destination) {
        destination.y = 0f;
        destination.z -= 4.5f;
        return destination;
    }

    public void Kill() {
        Destroy(this);
    }
}