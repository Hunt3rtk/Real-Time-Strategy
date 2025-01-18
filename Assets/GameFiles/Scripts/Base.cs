using Unity.VisualScripting;
using UnityEngine;

public class Base : Building {

        public GameObject gameOver;

        public override void Kill() {
        this.gameObject.transform.parent.GetChild(2).gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        gameOver.SetActive(true);
    }

}
