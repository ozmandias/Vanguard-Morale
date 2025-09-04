using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager instance;

    [Header("Allies")]
    public GameObject allyPrefab;
    public Transform []allySpawnLocations;
    public int allyCount = 0;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public Transform []enemySpawnLocations;
    public int enemyCount = 0;
    public bool combatAIsActive = false;
    
    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        allySpawnLocations = new Transform[transform.Find("AllySpawnLocations").childCount];
        for (int i = 0; i < allySpawnLocations.Length; i = i + 1)
        {
            allySpawnLocations[i] = transform.Find("AllySpawnLocations").GetChild(i);
        }

        enemySpawnLocations = new Transform[transform.Find("EnemySpawnLocations").childCount /*1*/];
        for (int i = 0; i < enemySpawnLocations.Length; i = i + 1)
        {
            enemySpawnLocations[i /*0*/] = transform.Find("EnemySpawnLocations").GetChild(i /*0*/);
        }
    }

    void Update()
    {
        if (allyCount < allySpawnLocations.Length)
        {
            SpawnAllies();
        }

        if (enemyCount < enemySpawnLocations.Length /*10*/)
        {
            SpawnEnemies();
        }

        if (combatAIsActive == false && enemyCount == enemySpawnLocations.Length)
        {
            AIManager.instance.SetupCombatAI();
            combatAIsActive = true;
        }
    }

    public void Spawn(GameObject spawnObject, Transform spawnLocation) {
        Instantiate(spawnObject, spawnLocation.position, Quaternion.identity);
    }

    void SpawnAllies() {
        foreach(Transform spawnLocation in allySpawnLocations) {
            Spawn(allyPrefab, spawnLocation);
            allyCount += 1;
        }
    }

    void SpawnEnemies() {
        foreach(Transform spawnLocation in enemySpawnLocations) {
            Spawn(enemyPrefab, spawnLocation);
            enemyCount += 1;
        }
	}
}