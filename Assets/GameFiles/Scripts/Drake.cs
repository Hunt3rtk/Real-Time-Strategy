using UnityEngine;

public class Drake : Unit {
    void Start() {
        commandSound = AudioManager.SoundType.Command;
        selectSound = AudioManager.SoundType.Select;
        spawnSound = AudioManager.SoundType.DrakeSpawn;
        attackSound = AudioManager.SoundType.DrakeAttack;
        deathSound = AudioManager.SoundType.DrakeDeath;
    }
}
