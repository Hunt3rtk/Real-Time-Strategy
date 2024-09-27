using UnityEngine;

public class PlacementValidity : MonoBehaviour {
     public GameManager gm;

     private int collidingCount;

     void Start() {
        collidingCount = 0;
     }

    void OnTriggerEnter(Collider other) {
        collidingCount += 1;
        Debug.Log($"Collision Enter Detection with count: {collidingCount} from: {other}");
        gm.TogglePlace(collidingCount);
    }

    void OnTriggerExit(Collider other) {
        collidingCount -= 1;
        Debug.Log($"Collision Exit Detection with count: {collidingCount} from: {other}");
        gm.TogglePlace(collidingCount);
    }
}
