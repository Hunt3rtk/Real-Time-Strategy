using System.Collections;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    internal Animator animator;

    public void Start() {
        animator = GetComponent<Animator>();
    }
}

[System.Serializable]
public class VisualEffect {
    public ParticleSystem particleSystem;
    public float delay;

    public IEnumerator Play() {
        if (particleSystem != null) {
            yield return new WaitForSeconds(delay);
            particleSystem.Play();
        }
    }

    public void Stop() {
        if (particleSystem != null) {
            particleSystem.Stop();
        }
    }
}
