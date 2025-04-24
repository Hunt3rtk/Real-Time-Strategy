using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour {
    [field: SerializeField]
    private float health;
    public float Health {
        get {
            return health;
        }

        set {
            health = value;
            if (health >= maxHealth) {
                health = maxHealth;
                Repaired();
            }
            if (health <= 0) Kill();
        }
    }
    public float maxHealth;

    internal BuildingAnimationPlayer buildingAnimationPlayer;

    void Awake() {
        
    }

    void Start() {
        Health = maxHealth;
    }

    public virtual void Repaired() {
        this.gameObject.SetActive(true);
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete));


        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public virtual void Kill() {
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);
    }
}
