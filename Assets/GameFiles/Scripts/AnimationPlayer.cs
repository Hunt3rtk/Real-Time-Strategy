using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class AnimationPlayer : MonoBehaviour {

    internal Animator animator;

    public void Start() {
        animator = GetComponent<Animator>();
    }
}

[System.Serializable]
public class VisualEffect {
    public ParticleSystem particleSystem;

    public UnityEngine.VFX.VisualEffect visualEffect;

    public float delay = 0f;

    public IEnumerator Play(Transform transform = null) {
        if (particleSystem != null) {
            yield return new WaitForSeconds(delay);
            if (transform != null) {
                particleSystem.transform.position = transform.position;
            }
            particleSystem.Play();

        }
        
        if (visualEffect != null) {
             yield return new WaitForSeconds(delay);
            if (transform != null) {
                visualEffect.transform.position = transform.position;
            }
            visualEffect.Play();
        }
    }

    public void Stop() {
        if (particleSystem != null) {
            particleSystem.Stop();
        }

        if (visualEffect != null) {
            visualEffect.Stop();
        }
    }
}
