using UnityEngine;

public class PlacementValidity : MonoBehaviour {
     public GameManager gm;

     private int collidingCount;

     void Start() {
        collidingCount = 0;
     }

    void OnTriggerEnter(Collider other) {
        if (other.name != "NavMeshBuffer" && other.name != "Prism") {
            ++collidingCount;
            Debug.Log($"Collision Enter Detection with count: {collidingCount} from: {other}");
        }

        if (other.name == "TreeStump") {
            --collidingCount;
            Debug.Log($"Collision Undo with count: {collidingCount} from: {other}");
            gm.stump = other;
        }

        if (collidingCount > 0) gm.validPlacement = false;

        //gm.TogglePlace(collidingCount);
    }

    void OnTriggerExit(Collider other) {
        if (other.name != "NavMeshBuffer" && other.name != "Prism") {
            --collidingCount;
            Debug.Log($"Collision Exit Detection with count: {collidingCount} from: {other}");
        }

        if (other.name == "TreeStump") {
            ++collidingCount;
            Debug.Log($"Collision Undo with count: {collidingCount} from: {other}");
            gm.stump = null;
        }
        
        if (collidingCount == 0) gm.validPlacement = true;

        //gm.TogglePlace(collidingCount);
    }
}
