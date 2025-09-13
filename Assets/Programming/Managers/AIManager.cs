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
        if (agentIndex != -1)
        {
            Vector3 reposition = CalculateLineUpPosition(aiList, agentIndex, destinationTransform);
            aiAgent.SetDestination(reposition);
        }
    }

    public Vector3 CalculateLineUpPosition(List<NavMeshAgent> aiList, int agentIndex, Transform destinationTransform) {
        int lineUpAgentsPerRow = 5;
        int rowsToLineUp = aiList.Count / lineUpAgentsPerRow;
        float lineUpSpacing = 10f;

        List<Vector3> lineUpPositions = new List<Vector3>();
        Vector3 lineUpPosition = Vector3.zero;

        int startAgentIndex = 0;
        int endAgentIndex = aiList.Count - 1;
        int middleAgentIndex = (startAgentIndex + endAgentIndex) / 2;
        // for(int row = 0; row < rowsToLineUp; row = row + 1) {
            // change startAgentIndex, endAgentIndex and middleAgentIndex
            /*int halfStartIndex = middleAgentIndex * row;
            int halfEndIndex = halfStartIndex + lineUpAgentsPerRow;
            int halfMiddleIndex = (halfStartIndex + halfEndIndex) / 2;
            Debug.Log("rowStart: " + halfStartIndex + ", rowMiddle: " + halfMiddleIndex + ", rowEnd: " + halfEndIndex);*/
            for (int agent = 0; agent < aiList.Count /*lineUpAgentsPerRow*/; agent = agent + 1) {
                Vector3 newLineUpPosition = Vector3.zero;
                if (agentIndex == middleAgentIndex) {
                    // center
                    Debug.Log("center: " + agentIndex);
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x,
                        destinationTransform.position.y,
                        destinationTransform.position.z
                    );
                } else if (agentIndex < middleAgentIndex) {
                    // left (1st), change end
                    Debug.Log("left index: " + agentIndex);
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x - ((agent + 1) * lineUpSpacing), // from middle as space factor
                        destinationTransform.position.y,
                        destinationTransform.position.z /*- (row * lineUpSpacing)*/
                    );
                    
                } else if (agentIndex > middleAgentIndex) {
                    // right (2nd), change start
                    Debug.Log("right index: " + agentIndex);
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x + ((agent - middleAgentIndex) * lineUpSpacing), // from middle as space factor. need to move only by 10 not 70
                        destinationTransform.position.y,
                        destinationTransform.position.z /*+ (row * lineUpSpacing)*/
                    );
                }
                lineUpPositions.Add(newLineUpPosition);
            }
        // }
        if (lineUpPositions.Count > agentIndex) {
            lineUpPosition = lineUpPositions[agentIndex];
        }

        return lineUpPosition;
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

    public void RemoveEnemy(Enemy enemy)
    {
        int removeLocation = 0;
        foreach (EnemyStruct enemyStruct in enemyStructs)
        {
            if (enemyStruct.enemy == enemy)
            {
                break;
            }
            removeLocation += 1;
        }

        enemyStructs.Remove(enemyStructs[removeLocation]);
    }

    IEnumerator SetCombatAILoopCoroutine(Enemy enemy)
    {
        if (enemyStructs.Count == 0)
        {
            StopCoroutine(SetCombatAILoopCoroutine(null));
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        Enemy combatingEnemy = RandomEnemyExcluding(enemy);
        if (!combatingEnemy) combatingEnemy = RandomEnemy();
        if (!combatingEnemy) yield break;

        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsRetreating == false);
        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsPlayerTarget == false);
        yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsStunned == false);

        if (combatingEnemy.GetInfo().aiType == AIType.CombatAI)
        {
            combatingEnemy.personCombat.SetAttackPlayer();
            yield return new WaitUntil(() => combatingEnemy.personCombat.enemyIsNearPlayer == true && combatingEnemy.personCombat.enemyIsAttacking == false || combatingEnemy.GetInfo().isDead); // yield return new WaitForSeconds(Random.Range(0, 5f /*0.5f*/)); // change waitforseconds()
            combatingEnemy.personCombat.SetRetreatFromPlayer();
        }

        if (enemyStructs.Count > 0)
        {
            CombatAILoopCoroutine = StartCoroutine(SetCombatAILoopCoroutine(combatingEnemy)); // recursion
        }
    }
    
    void OnGUI()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;

        if (SceneManager.instance.GetCurrentScene().name == "game") {
            if (GUI.Button(new Rect(30, 250, 350, 100), "Agents to Destination", buttonStyle)) {
                foreach(Friend soldier in GameManager.instance.soldierList) {
                    soldier.personAgent.isStopped = false;
                    AgentRepositionAtDestination(PersonType.Friend, soldier.personAgent, soldier.destination);
                }

                foreach (Enemy enemy in GameManager.instance.enemyList) {
                    enemy.personAgent.isStopped = false;
                    AgentRepositionAtDestination(PersonType.Enemy, enemy.personAgent, enemy.destination);
                }
            }
        }

        if(SceneManager.instance.GetCurrentScene().name == "lab") {
            if (GUI.Button(new Rect(30, 250, 250, 100), "Run CombatAI", buttonStyle)) {
                SetupCombatAI();
            }
        }
    }
}