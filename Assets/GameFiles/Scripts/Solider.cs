using UnityEngine;
using UnityEngine.AI;

public class Solider : Unit {

    void Start() {
        selectSound = AudioManager.SoundType.Select;
        commandSound = AudioManager.SoundType.Command;
        attackSound = AudioManager.SoundType.SoliderAttack;
        deathSound = AudioManager.SoundType.SoliderDeath;
    }
}
