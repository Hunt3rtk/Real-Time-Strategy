using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField]
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

    void Start() {
        Health = maxHealth;
    }

    public void Repaired() {
        this.gameObject.transform.parent.GetChild(2).gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    private void Kill() {
        this.gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
