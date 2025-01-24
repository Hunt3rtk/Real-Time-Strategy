using Unity.VisualScripting;
using UnityEngine;

public class WorldToScreenFollow : MonoBehaviour
{

    [HideInInspector]
    public Unit unit;

    // Update is called once per frame
    void Update()
    {
        if (unit.gameObject.IsDestroyed()) Destroy(gameObject);
        
        gameObject.transform.position = Camera.main.WorldToScreenPoint(unit.gameObject.transform.position);
    }
}
