using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnManager : MonoBehaviour {

    public GameObject[] spawners;

    [SerializeField]
    public List<Wave> waves;

    public class Batch {
        public int enemyTanks;
        public int enemySoldiers;
        public int spawner;
    }
    
    public class Wave {
        public float restTime;
        public float spawnTime;
        public Batch[] batches;
    }

}
