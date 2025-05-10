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

    private DamageFlash damageFlash;

    [HideInInspector]
    public float strikeDelay = 0;

    void Start() {
        Health = maxHealth;

        damageFlash = GetComponent<DamageFlash>();;
    }

    public virtual void Repaired() {
        this.gameObject.SetActive(true);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(false);
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete));


        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public virtual void Kill() {
        //StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingDestroyed));
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);
        this.transform.parent.Find("ConstructionSite").GetComponent<AnimateOnStart>().PlayDestroyed();

        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayDestroyed(transform);
        
    }
}
