using UnityEngine;

public class BuildingAnimationPlayer : AnimationPlayer {
    [SerializeField]
    internal VisualEffect PlaceEffect;


    public void PlayPlace(Transform transform = null) {
        if (PlaceEffect != null) {
            StartCoroutine(PlaceEffect.Play(transform));
        }
    }
}