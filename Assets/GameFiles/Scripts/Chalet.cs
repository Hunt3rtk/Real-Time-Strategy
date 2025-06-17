
using UnityEngine;

public class Chalet : Building {

    void Awake()
    {
        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots += 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);

        damageFlash = GetComponent<DamageFlash>();

        buildingAnimationPlayer = transform.GetComponentInParent<BuildingAnimationPlayer>();

        if (transform.Find("SparkleParticleEffect") != null)
        {
            buildingAnimationPlayer.FinishedEffect.particleSystem = transform.Find("SparkleParticleEffect").GetComponent<ParticleSystem>();
        }

        if (transform.Find("BuildingDestroyed") != null)
        {
            buildingAnimationPlayer.DestroyedEffect.particleSystem = transform.Find("BuildingDestroyed").GetComponent<ParticleSystem>();
        }
        
        Health = maxHealth;
    }

    public override void Repaired() {
        this.gameObject.SetActive(true);
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete));

        buildingAnimationPlayer = transform.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public override void Kill() {
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);

        buildingAnimationPlayer.PlayDestroyed(transform);

        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots -= 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);
    }
}