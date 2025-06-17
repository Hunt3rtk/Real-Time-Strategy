using UnityEngine;

public class Building : MonoBehaviour {
    [field: SerializeField]
    private float health;
    public float Health {
        get {
            return health;
        }

        set {

            if (value < health && damageFlash != null) {
                StartCoroutine(damageFlash.Flash(strikeDelay));
            }

            health = value;
            
            if (health >= maxHealth) {

                health = maxHealth;

                Repaired();

            } else if (health <= 0) Kill();
        }
    }
    public float maxHealth;

    public BuildingAnimationPlayer buildingAnimationPlayer;

    public DamageFlash damageFlash;

    [HideInInspector]
    public float strikeDelay = 0;

    void Awake()
    {

        damageFlash = GetComponent<DamageFlash>();

        if (transform.GetComponentInParent<BuildingAnimationPlayer>() != null) {
            buildingAnimationPlayer = transform.GetComponentInParent<BuildingAnimationPlayer>();
        }
        
        if (transform.Find("SparkleParticleEffect") != null) {
            buildingAnimationPlayer.FinishedEffect.particleSystem = transform.Find("SparkleParticleEffect").GetComponent<ParticleSystem>();
        }

        if (transform.Find("BuildingDestroyed") != null) {
            buildingAnimationPlayer.DestroyedEffect.particleSystem = transform.Find("BuildingDestroyed").GetComponent<ParticleSystem>();
        }
        
        Health = maxHealth;
    }

    public virtual void Repaired() {
        this.gameObject.SetActive(true);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(false);

        buildingAnimationPlayer = transform.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public virtual void Kill() {
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);

        buildingAnimationPlayer.PlayDestroyed(transform);
        
    }
}
