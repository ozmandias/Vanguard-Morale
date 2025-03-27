using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager instance;
    public GameObject enemyPrefab;
    public Transform []enemySpawnLocations;
    public int enemyCount = 0;
    
    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        enemySpawnLocations = new Transform[1];
        enemySpawnLocations[0] = transform.GetChild(0);
    }

    void Update() {
        if(enemyCount < 10) {
            SpawnEnemies();
        }
    }

    public void Spawn(GameObject spawnObject, Transform spawnLocation) {
        Instantiate(spawnObject, spawnLocation.position, Quaternion.identity);
    }

    void SpawnEnemies() {
        foreach(Transform spawnLocation in enemySpawnLocations) {
            Spawn(enemyPrefab, spawnLocation);
            enemyCount += 1;
        }
	}
}