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

    internal BuildingAnimationPlayer buildingAnimationPlayer;

    public DamageFlash damageFlash;

    [HideInInspector]
    public float strikeDelay = 0;

    void Start() {

        damageFlash = GetComponent<DamageFlash>();
        
        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();

         Health = maxHealth;
        
        if (transform.parent.Find("SparkleParticleEffect") != null){
            buildingAnimationPlayer.FinishedEffect.particleSystem = transform.parent.Find("SparkleParticleEffect").GetComponent<ParticleSystem>();
        }

        if (transform.parent.Find("BuildingDestroyed") != null) {
            buildingAnimationPlayer.DestroyedEffect.particleSystem = transform.parent.Find("BuildingDestroyed").GetComponent<ParticleSystem>();
        }
    }

    public virtual void Repaired() {
        this.gameObject.SetActive(true);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(false);
        
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public virtual void Kill() {
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);

        buildingAnimationPlayer.PlayDestroyed(transform);
        
    }
}
