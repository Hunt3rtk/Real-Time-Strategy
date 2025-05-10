
public class Chalet : Building {

    void Start() {
        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots += 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);
    }

    public override void Repaired() {
        this.gameObject.SetActive(true);
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete));

        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public override void Kill() {
        //StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingDestroyed));
        this.gameObject.SetActive(false);
        this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);
        this.transform.parent.Find("ConstructionSite").GetComponent<AnimateOnStart>().PlayDestroyed();

        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayDestroyed(transform);

        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots -= 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);
    }
}