using System.Collections;
using System.Linq;
using States;
using UnityEngine;

public class Guard : MonoBehaviour {

    private Transform home;
    private Unit unit;

    void Awake() {
        unit = this.gameObject.GetComponent<Unit>();
        home = GameObject.Find("Base").transform;
    }

    public void StartSearch() {
        if (unit.state == Unit.State.Idle) {
            StopAllCoroutines();
            StartCoroutine(CheckVisibility(unit.visibilityRange));
        }
    }

    private IEnumerator CheckVisibility(float visibilityRange) {

        yield return new WaitForSeconds(.5f);

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, visibilityRange, LayerMask.GetMask("EnemyUnits"));

        if (hitColliders.Length > 0)
        {
            hitColliders = hitColliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
            unit.AttackStandAlone(hitColliders[0]);
            yield break;
        }

        StartSearch();
    }
}