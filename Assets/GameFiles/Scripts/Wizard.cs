using UnityEngine;

public class Wizard : Unit {
    void Start() {
        selectSound = AudioManager.SoundType.Select;
        commandSound = AudioManager.SoundType.Command;
        spawnSound = AudioManager.SoundType.WizardSpawn;
        attackSound = AudioManager.SoundType.WizardAttack;
        deathSound = AudioManager.SoundType.WizardDeath;
    }

    public override void SetStateAttack( Unit targetUnit, Building targetBuilding = null) {
        transform.LookAt((targetBuilding != null) ? targetBuilding.transform.position : targetUnit.transform.position);
        StartCoroutine(AudioManager.Instance.Play(attackSound));
        if (targetBuilding != null) {
            animationPlayer.PlayAttack(targetBuilding.transform);
            targetBuilding.Health -= damage;
        } else {
            animationPlayer.PlayAttack(targetUnit.transform);
            targetUnit.Health -= damage;
        }
    }
}
