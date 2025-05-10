using UnityEngine;

public class BuildingAnimationPlayer : AnimationPlayer {
    [SerializeField]
    internal VisualEffect PlaceEffect;

    [SerializeField]
    internal VisualEffect FinishedEffect;

    [SerializeField]
    internal VisualEffect DestroyedEffect;


    public void PlayPlace(Transform transform = null) {
        if (PlaceEffect != null) {
            StartCoroutine(PlaceEffect.Play(transform));
        }
    }

    public void PlayFinished(Transform transform = null) {
        if (FinishedEffect != null) {
            StartCoroutine(FinishedEffect.Play(transform));
        }
    }

    public void PlayDestroyed(Transform transform = null) {
        if (DestroyedEffect != null) {
            StartCoroutine(DestroyedEffect.Play(transform));
        }
    }
}