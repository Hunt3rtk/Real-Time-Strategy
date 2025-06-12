using Unity.VisualScripting;
using UnityEngine;

public class Base : Building {

        public GameObject gameOver;

        public override void Kill() {
            this.gameObject.SetActive(false);
            this.transform.parent.Find("ConstructionSite").gameObject.SetActive(true);

            buildingAnimationPlayer.PlayDestroyed(transform);

            gameOver.SetActive(true);
    }

}
