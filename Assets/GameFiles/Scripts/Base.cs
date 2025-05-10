using Unity.VisualScripting;
using UnityEngine;

public class Base : Building {

        public GameObject gameOver;

        public override void Kill() {
            //StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingDestroyed));
            this.gameObject.SetActive(false);
            this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);
            this.transform.parent.Find("ConstructionSite").GetComponent<AnimateOnStart>().PlayDestroyed();

            buildingAnimationPlayer = transform.parent.GetComponentInParent<BuildingAnimationPlayer>();
            buildingAnimationPlayer.PlayDestroyed(transform);

            gameOver.SetActive(true);
    }

}
