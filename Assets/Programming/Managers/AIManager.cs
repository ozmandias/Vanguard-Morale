using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;

    public List<NavMeshAgent> soldierAIList;
    public List<NavMeshAgent> personAIList;
    public List<NavMeshAgent> enemyAIList;

    public List<EnemyStruct> enemyStructs = new List<EnemyStruct>();

    public float RadiusAroundTarget = 8f;
    public float DistanceAroundDestination = 8f;

    private Coroutine CombatAILoopCoroutine;

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
        /*Enemy[] enemyObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy enemyObject in enemyObjects)
        {
            EnemyStruct enemyStruct = new EnemyStruct();
            enemyStruct.enemy = enemyObject;
            enemyStruct.available = true;
            enemyStructs.Add(enemyStruct);
        }

        StartAI();*/
    }

    public void AddToList(PersonType personType, NavMeshAgent aiAgent)
    {
        switch (personType)
        {
            case PersonType.Friend:
                soldierAIList.Add(aiAgent);
                break;
            case PersonType.Enemy:
                enemyAIList.Add(aiAgent);
                break;
            default:
                personAIList.Add(aiAgent);
                break;
        }
    }

    public void RemoveFromList(PersonType personType, NavMeshAgent aiAgent)
    {
        switch (personType)
        {
            case PersonType.Friend:
                soldierAIList.Remove(aiAgent);
                break;
            case PersonType.Enemy:
                enemyAIList.Remove(aiAgent);
                break;
            default:
                personAIList.Remove(aiAgent);
                break;
        }
    }

    public bool ListsContain(NavMeshAgent aiAgent)
    {
        if (soldierAIList.Contains(aiAgent) || personAIList.Contains(aiAgent) || enemyAIList.Contains(aiAgent))
        {
            return true;
        }
        return false;
    }

    public void AgentCircleTarget(PersonType personType, NavMeshAgent aiAgent, Transform targetTransform, CircleType circleType)
    {
        List<NavMeshAgent> aiList;
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;
        int agentIndex = -1;

        switch (personType)
        {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }
        agentIndex = aiList.IndexOf(aiAgent);

        if (agentIndex != -1)
        {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Cos(circleMultiplier * Mathf.PI * agentIndex / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * agentIndex / aiList.Count)
            );
            aiAgent.SetDestination(circleDestination);
        }
    }

    public void AgentRepositionAtDestination(PersonType personType, NavMeshAgent aiAgent, Transform destinationTransform)
    {
        List<NavMeshAgent> aiList;
        int agentIndex = -1;

        switch (personType)
        {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }
        agentIndex = aiList.IndexOf(aiAgent);

        if (agentIndex != -1 /*&& agentIndex < 5*/)
        {
            Vector3 reposition = new Vector3(
                destinationTransform.position.x + DistanceAroundDestination * Mathf.Cos(Mathf.PI * agentIndex / aiList.Count),
                destinationTransform.position.y,
                destinationTransform.position.z + DistanceAroundDestination * Mathf.Sin(Mathf.PI * agentIndex / aiList.Count)
            );
            aiAgent.SetDestination(reposition);
        } /*else {
            // move to next line of position
        }*/
    }

    public void AgentsCircleTarget(PersonType personType, Transform targetTransform, CircleType circleType)
    {
        List<NavMeshAgent> aiList;
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;

        switch (personType)
        {
            case PersonType.Friend:
                aiList = soldierAIList;
                break;
            case PersonType.Enemy:
                aiList = enemyAIList;
                break;
            default:
                aiList = personAIList;
                break;
        }

        for (int i = 0; i < aiList.Count; i = i + 1)
        {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Cos(circleMultiplier * Mathf.PI * i / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count)
            );
            aiList[i].SetDestination(circleDestination);
        }
    }

    public void ListCircleTarget(List<NavMeshAgent> aiList, Transform targetTransform, CircleType circleType)
    {
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;
        for (int i = 0; i < aiList.Count; i = i + 1)
        {
            Vector3 circleDestination = new Vector3(
                targetTransform.position.x + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count)
            );
            aiList[i].SetDestination(circleDestination);
        }
    }

    void OnGUI()
    {
        /*if(GUI.Button(new Rect(10, 10, 150, 50), "AI To Target")) {
            foreach(Friend soldier in GameManager.instance.soldierList) {
                if(soldier.target) AIManager.instance.AgentCircleTarget(soldier.GetInfo().personType, soldier.personAgent, soldier.target.transform, CircleType.Semicircle);
            }

            foreach(Enemy enemy in GameManager.instance.enemyList) {
                if(enemy.target) AIManager.instance.AgentCircleTarget(enemy.GetInfo().personType, enemy.personAgent, enemy.target.transform, CircleType.Semicircle);
            }
        }*/

        /*if(GUI.Button(new Rect(10, 80, 150, 50), "AI To Destination")) {
            foreach(Friend soldier in GameManager.instance.soldierList) {
                AIManager.instance.AgentRepositionAtDestination(soldier.GetInfo().personType, soldier.personAgent, soldier.destination);
            }

            foreach(Enemy enemy in GameManager.instance.enemyList) {
                AIManager.instance.AgentRepositionAtDestination(enemy.GetInfo().personType, enemy.personAgent, enemy.destination);
            }
        }*/
    }

    void StartAI()
    {
        CombatAILoopCoroutine = StartCoroutine(SetCombatAILoopCoroutine(null));
    }

    public void SetupCombatAI()
    {
        Enemy[] enemyObjects = FindObjectsOfType<Enemy>();
        foreach (Enemy enemyObject in enemyObjects)
        {
            EnemyStruct enemyStruct = new EnemyStruct();
            enemyStruct.enemy = enemyObject;
            enemyStruct.available = true;
            enemyStructs.Add(enemyStruct);
        }

        StartAI();
    }

    public Enemy RandomEnemy()
    {
        List<int> randomLocationList = new List<int>();
        for (int i = 0; i < enemyStructs.Count; i = i + 1)
        {
            if (enemyStructs[i].enemy.personCombat.available)
            {
                randomLocationList.Add(i);
            }
        }

        if (randomLocationList.Count == 0)
        {
            return null;
        }

        Enemy randomEnemy;
        int randomLocation = Random.Range(0, randomLocationList.Count);
        randomEnemy = enemyStructs[randomLocationList[randomLocation]].enemy;
        randomLocationList.Clear();
        return randomEnemy;
    }

    public Enemy RandomEnemyExcluding(Enemy excludingEnemy)
    {
        List<int> randomLocationList = new List<int>();
        for (int i = 0; i < enemyStructs.Count; i = i + 1)
        {
            if (enemyStructs[i].enemy.personCombat.available && enemyStructs[i].enemy != excludingEnemy)
            {
                randomLocationList.Add(i);
            }
        }

        if (randomLocationList.Count == 0)
        {
            return null;
        }

        Enemy randomEnemy;
        int randomLocation = Random.Range(0, randomLocationList.Count);
        randomEnemy = enemyStructs[randomLocationList[randomLocation]].enemy;
        randomLocationList.Clear();
        return randomEnemy;
    }

    public void SetEnemyAvailable(Enemy enemy, bool availiability)
    {
        EnemyStruct enemyStructToChange;
        foreach (EnemyStruct enemyStruct in enemyStructs)
        {
            if (enemyStruct.enemy == enemy)
            {
                enemyStructToChange = enemyStruct;
                enemyStructToChange.available = availiability;
                break;
            }
        }
    }
    
    IEnumerator SetCombatAILoopCoroutine(Enemy enemy)
    {
        if (enemyStructs.Count == 0)
        {
            StopCoroutine(SetCombatAILoopCoroutine(null));
            yield break;
        }

        Debug.Log("Total enemyStructs: " + enemyStructs.Count);

        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        Enemy combatingEnemy = RandomEnemyExcluding(enemy);
        if (!combatingEnemy) combatingEnemy = RandomEnemy();
        if (!combatingEnemy) yield break;

        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsRetreating == false);
        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsPlayerTarget == false);
        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsStunned == false);

        combatingEnemy.personCombat.SetAttackPlayer();
        yield return new WaitForSeconds(Random.Range(0, 5f /*0.5f*/)); // change waitforseconds()
        combatingEnemy.personCombat.SetRetreatFromPlayer();

        if (enemyStructs.Count > 0)
        {
            CombatAILoopCoroutine = StartCoroutine(SetCombatAILoopCoroutine(combatingEnemy)); // recursion
        }
    }
}