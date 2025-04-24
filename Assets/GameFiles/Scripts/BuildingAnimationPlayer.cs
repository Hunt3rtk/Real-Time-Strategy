using UnityEngine;

public class BuildingAnimationPlayer : AnimationPlayer {
    [SerializeField]
    internal VisualEffect PlaceEffect;

    [SerializeField]
    internal VisualEffect FinishedEffect;


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
}