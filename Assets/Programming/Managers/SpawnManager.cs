using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool spawnManagerRunning = false;

    [Header("Allies")]
    public GameObject allyPrefab;
    public Vector3 []allySpawnLocations;
    public int allyCount = 0;

    [Header("Normal")]
    public GameObject characterPrefab;
    public Vector3 []spawnLocations;
    public int spawnCount = 0;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public Vector3 []enemySpawnLocations;
    public int enemyCount = 0;

    public static SpawnManager instance;

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
        spawnManagerRunning = true;

        if(spawnLocations.Length == 0) {
            var spawnLocationsObject = GameObject.Find("SpawnLocations");
            spawnLocations = new Vector3[spawnLocationsObject.transform.childCount];
            for (int i = 0; i < spawnLocations.Length; i = i + 1)
            {
                spawnLocations[i] = spawnLocationsObject.transform.GetChild(i).position;
            }
        }

        if(allySpawnLocations.Length == 0) {
            var allySpawnLocationsObject = GameObject.Find("AllySpawnLocations");
            allySpawnLocations = new Vector3[allySpawnLocationsObject.transform.childCount];
            for (int i = 0; i < allySpawnLocations.Length; i = i + 1)
            {
                allySpawnLocations[i] = allySpawnLocationsObject.transform.GetChild(i).position;
            }
        }

        if(enemySpawnLocations.Length == 0) {
            var enemySpawnLocationsObject = GameObject.Find("EnemySpawnLocations");
            enemySpawnLocations = new Vector3[enemySpawnLocationsObject.transform.childCount /*1*/];
            for (int i = 0; i < enemySpawnLocations.Length; i = i + 1)
            {
                enemySpawnLocations[i /*0*/] = enemySpawnLocationsObject.transform.GetChild(i /*0*/).position;
            }
        }

        SpawnAll();
        
        /*if (enemyCount == enemySpawnLocations.Length)
        {
            AIManager.instance.SetupCombatAI();
        }*/
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