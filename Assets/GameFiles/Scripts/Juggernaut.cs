using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Juggernaut : Unit {
    void Start() {
        selectSound = AudioManager.SoundType.Select;
        commandSound = AudioManager.SoundType.Command;
        spawnSound = AudioManager.SoundType.JuggernautSpawn;
        attackSound = AudioManager.SoundType.JuggernautAttack;
        deathSound = AudioManager.SoundType.JuggernautDeath;
    }
}

