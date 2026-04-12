using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("Combat Manager Settings")]
    public Vector3 []spawnLocations;

    [Header("Allies")]
    public GameObject allyPrefab;
    public Vector3 []allySpawnLocations;
    public int allyCount = 0;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public Vector3 []enemySpawnLocations;
    public int enemyCount = 0;
    public bool combatAIsActive = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        spawnLocations = new Vector3[transform.Find("SpawnLocations").childCount];
        for (int i = 0; i < spawnLocations.Length; i = i + 1)
        {
            spawnLocations[i] = transform.Find("SpawnLocations").GetChild(i).position;
        }

        allySpawnLocations = new Vector3[transform.Find("AllySpawnLocations").childCount];
        for (int i = 0; i < allySpawnLocations.Length; i = i + 1)
        {
            allySpawnLocations[i] = transform.Find("AllySpawnLocations").GetChild(i).position;
        }

        enemySpawnLocations = new Vector3[transform.Find("EnemySpawnLocations").childCount /*1*/];
        for (int i = 0; i < enemySpawnLocations.Length; i = i + 1)
        {
            enemySpawnLocations[i /*0*/] = transform.Find("EnemySpawnLocations").GetChild(i /*0*/).position;
        }

        SpawnAll();
        
        if (enemyCount == enemySpawnLocations.Length)
        {
            AIManager.instance.SetupCombatAI();
        }
    }

    void Update()
    {
    }

    public void Spawn(GameObject spawnObject, Vector3 spawnLocation)
    {
        Instantiate(spawnObject, spawnLocation, Quaternion.identity);
    }

    public void Spawn(GameObject spawnObject, Vector3 spawnPosition, Quaternion spawnRotation) {
        Instantiate(spawnObject, spawnPosition, spawnRotation);
    }

    void SpawnAll()
    {
        SpawnAllies();
        SpawnEnemies();
    }

    void SpawnAllies()
    {
        foreach (Vector3 spawnLocation in allySpawnLocations)
        {
            Spawn(allyPrefab, spawnLocation);
            allyCount += 1;
        }
    }

    void SpawnEnemies()
    {
        foreach (Vector3 spawnLocation in enemySpawnLocations)
        {
            Spawn(enemyPrefab, spawnLocation);
            enemyCount += 1;
        }
    }
}