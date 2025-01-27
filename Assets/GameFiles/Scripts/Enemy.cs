using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform home;
    private Unit unit;
    private float visibilityRange;



    void Awake() {
        unit = this.gameObject.GetComponent<Unit>();
        home = GameObject.Find("BaseParent").transform;
        visibilityRange = unit.visibilityRange;
    }

    void Start() {
        AttackBase(home.position);
    }

    private void AttackBase(Vector3 homePosition) {
        StartCoroutine(unit.Move(homePosition));
        StartCoroutine(CheckVisibility(visibilityRange));
    }

    private IEnumerator CheckVisibility(float visibilityRange) {

        yield return new WaitForSeconds(.5f);

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, visibilityRange, LayerMask.GetMask("Units", "Building"));

        if (hitColliders.Length <= 0) {
            StartCoroutine(CheckVisibility(visibilityRange));
            yield break;
        }

        hitColliders = hitColliders.OrderBy(x => x.tag switch {
            "Unit" => 0,
            "Building" => 1,
            "Base" => 1,
            "Barracks" => 1,
            _ => 2
        }).ThenBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();

        yield return unit.Attack(hitColliders[0]);
        AttackBase(home.position);
        yield break;
    }
}
