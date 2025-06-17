using UnityEngine;

public class Tree : MonoBehaviour {

    public bool taken = false;

    public void ChopTree()
    {
        this.gameObject.SetActive(false);
        this.transform.parent.GetChild(1).gameObject.SetActive(true);
        GameManager gm = FindAnyObjectByType<GameManager>();
        gm.UpdateNavMesh();
    }
}
