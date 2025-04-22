using UnityEngine;

public class AnimateOnStart : MonoBehaviour {

    private Animation a;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        a = GetComponent<Animation>();
        
        if (a != null) a.Play();
    }
}
