using System.Collections;
using UnityEngine;

public class UnitAnimationPlayer : AnimationPlayer {

    // Particle Effects

    [SerializeField]
    internal VisualEffect AttackEffect;

    [field:SerializeField]
    internal VisualEffect WalkEffect;

    [field:SerializeField]
    internal VisualEffect WorkEffect;

    [field:SerializeField]
    internal VisualEffect DeathEffect;


    // Function to play attack animation
    public void PlayAttack(Transform transform = null) {
        if (animator != null)
        {
            if (AttackEffect != null) {
                StartCoroutine(AttackEffect.Play(transform));
            }
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }
    }

    // Function to play walk animation
    public void PlayWalk() {
        if (animator != null) {
            if (WalkEffect != null) {
                StartCoroutine(WalkEffect.Play());
            }
           animator.SetBool("isWalking", true);
        }
    }

        public void PlayWork() {
        if (animator != null)
        {
            if (WorkEffect != null) {
                StartCoroutine(WorkEffect.Play());
            }
            animator.SetBool("isWorking", true);
        }
    }

    public void PlayDeath() {
        if (animator != null) {
            if (DeathEffect != null) {
                StartCoroutine(DeathEffect.Play());
            }
            animator.ResetTrigger("Death");
            animator.SetTrigger("Death");
        }
    }

    // Function to stop walk animation
    public void StopWalk() {
        if (animator != null) {
            if (WalkEffect != null) {
                WalkEffect.Stop();
            }
            animator.SetBool("isWalking", false);
        }
    }

    public void StopWork() {
        if (animator != null) {
            if (WorkEffect != null) {
                WorkEffect.Stop();
            }
            animator.SetBool("isWorking", false);
        }
    }
}
