using UnityEngine;

public class Tree : MonoBehaviour {
    public void ChopTree() {
        this.gameObject.SetActive(false);
        GameManager gm = FindAnyObjectByType<GameManager>();
        gm.UpdateNavMesh();
    }
}
