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
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingComplete));
        if (FinishedEffect != null)
        {
            StartCoroutine(FinishedEffect.Play(transform));
        }
    }

    public void PlayDestroyed(Transform transform = null) {
        StartCoroutine(AudioManager.Instance.Play(AudioManager.SoundType.BuildingDestroyed));
        if (DestroyedEffect != null)
        {
            StartCoroutine(DestroyedEffect.Play(transform));
        }
    }
}