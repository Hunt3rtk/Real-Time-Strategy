using UnityEngine;

public class Chalet : Building {
    public override void Repaired() {
        this.gameObject.SetActive(true);
        AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete);
        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots += 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);

        buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
        buildingAnimationPlayer.PlayFinished(transform);
    }

    public override void Kill() {
        this.gameObject.SetActive(false);
        int unitSlots = FindAnyObjectByType<GameManager>().unitSlots -= 2;
        FindAnyObjectByType<HUDManager>().UpdateUnitSlots(unitSlots);
    }
}