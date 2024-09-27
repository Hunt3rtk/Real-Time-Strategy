using UnityEngine;

public class WorldToScreenFollow : MonoBehaviour
{

    [HideInInspector]
    public Unit unit;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Camera.main.WorldToScreenPoint(unit.gameObject.transform.position);
    }
}
