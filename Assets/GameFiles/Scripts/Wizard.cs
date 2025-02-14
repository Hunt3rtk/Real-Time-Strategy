using UnityEngine;

public class Wizard : Unit {
    void Start() {
        selectSound = AudioManager.SoundType.Select;
        commandSound = AudioManager.SoundType.Command;
        spawnSound = AudioManager.SoundType.WizardSpawn;
        attackSound = AudioManager.SoundType.WizardAttack;
        deathSound = AudioManager.SoundType.WizardDeath;
    }
}
