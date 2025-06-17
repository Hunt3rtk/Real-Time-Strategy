using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static AudioManager;

public class WaveSpawnManager : MonoBehaviour {

    [SerializeField]
    public Text timer;

    [SerializeField]
    public UnitDataBase units;

    private float time;

    private int waveIndex = 0;

    private bool musicChange = false;

    public GameObject victoryScreen;

    public Building enemyBase;

    public GameObject[] spawners;

    [SerializeField]
    public List<Wave> waves;
    
    [System.Serializable]
    public class Wave {
        public float restTime;
        public float waveTime;
        public Batch[] batches;
    }

    [System.Serializable]
    public class Batch {

        public int enemyOne;
        public int enemyTwo;
        public int enemyThree;
        public int spawner;
    }

    void FixedUpdate() {
        if (FindFirstObjectByType<Enemy>() == null && waves.Count <= waveIndex || enemyBase.enabled == false) {
            Time.timeScale = 0;
            victoryScreen.SetActive(true);
        } else if (FindFirstObjectByType<Enemy>() == null) {
            if (!musicChange) {
                AudioManager.Instance.ChangeMusic(SoundType.Music_Preparation);
                musicChange = true;
            }
            time += Time.deltaTime;
            timer.text = (waves[waveIndex].restTime - (int)time).ToString();
            if (time >= waves[waveIndex].restTime) {
                AudioManager.Instance.ChangeMusic(SoundType.Music_Battle);
                musicChange = false;
                SpawnWave();
                waveIndex++;
                time = 0;
                timer.text = time.ToString();
            }
        }
    }

    void SpawnWave() {
        foreach (Batch batch in waves[waveIndex].batches) {
            if (batch.spawner >= spawners.Length || spawners[batch.spawner].GetComponentInChildren<Building>() == null) {
                continue;
            }
            for (int i = 0; i < batch.enemyOne; i++) {
                SpawnUnit(0, spawners[batch.spawner].transform);
            }
            for (int i = 0; i < batch.enemyTwo; i++) {
                SpawnUnit(1, spawners[batch.spawner].transform);
            }
             for (int i = 0; i < batch.enemyThree; i++) {
                SpawnUnit(2, spawners[batch.spawner].transform);
            }
        }
    }

    //Instantiates a Unit
    public void SpawnUnit(int id, Transform location) {
        GameObject newUnit = Instantiate(units.unitDatas[id].prefab);
        Unit unit = newUnit.GetComponent<Unit>();
        
        NavMeshHit hit;
        NavMesh.SamplePosition(location.position, out hit, 100f, NavMesh.AllAreas);
        unit.agent.Warp(new Vector3(hit.position.x, hit.position.y+.5f, hit.position.z));
    }

}
