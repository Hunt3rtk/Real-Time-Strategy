using UnityEngine;
using UnityEngine.AI;

public class UnitCollide : MonoBehaviour
{

    private Unit unit;
    private NavMeshObstacle obstacle;

    void Start() {
        unit = GetComponent<Unit>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.GetMask("Units") && other.transform.position.x > transform.position.x) {
            unit.agent.isStopped = true;
            unit.agent.enabled = false;

            obstacle.enabled = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.GetMask("Units")) {

            obstacle.enabled = false;

            unit.agent.enabled = true;
            unit.agent.isStopped = false;
        }
    }

}
