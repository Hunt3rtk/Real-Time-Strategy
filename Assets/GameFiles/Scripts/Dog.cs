using UnityEngine;

public class Dog: Unit {

    void Start() {
        //selectSound = AudioManager.SoundType.Select;
        //commandSound = AudioManager.SoundType.Command;
        attackSound = AudioManager.SoundType.SoliderAttack;
        deathSound = AudioManager.SoundType.SoliderDeath;
    }
}
