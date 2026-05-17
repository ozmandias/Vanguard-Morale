using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour { // for AI group
    public static AIManager instance;

    public List<NavMeshAgent> friendAIList;
    // public List<NavMeshAgent> companionAIList;
    public List<NavMeshAgent> personAIList;
    public List<NavMeshAgent> enemyAIList;
    // public List<NavMeshAgent> bossAIList;
    
    public List<Vector3> soldierPositions;
    public List<Vector3> enemyPositions;
    public List<Vector3> personPositions;
    
    public float RadiusAroundTarget = 10f;
    public float DistanceAroundDestination = 10f;

    public List<EnemyStruct> enemyStructs = new List<EnemyStruct>();
    private Coroutine CombatAILoopCoroutine;
    
    public AgentsCircleEvent OnAgentsCircle = new AgentsCircleEvent();
    public CombatAISetupEvent OnCombatAISetup = new CombatAISetupEvent();
    public AIListRegisterEvent OnAIListRegister = new AIListRegisterEvent();
    public AIListUnregisterEvent OnAIListUnregister = new AIListUnregisterEvent();

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
        OnAgentsCircle.AddListener((aiList, transform, circleType) => ListCircleTarget(aiList, transform, circleType));
        
        // need to trigger this after SpawnManager. (update: Do not rely on SpawnManager and use Events)
        /*Enemy []enemies = FindObjectsOfType<Enemy>();
        if(enemies.Length > 0) {
            foreach (Enemy enemy in enemies)
            {
                EnemyStruct enemyStruct = new EnemyStruct();
                enemyStruct.enemy = enemy;
                enemyStruct.available = true;
                enemyStructs.Add(enemyStruct);
            }

            StartCombatAI();
        }*/

        OnCombatAISetup.AddListener(() => SetupCombatAI());
        OnAIListRegister.AddListener((personType, navMeshAgent) => AIListRegister(personType, navMeshAgent));
        OnAIListUnregister.AddListener((personType, navMeshAgent) => AIListUnregister(personType, navMeshAgent));
    }

    public void AddToAIList(PersonType personType, NavMeshAgent aiAgent)
    {
        switch (personType)
        {
            case PersonType.Friend:
            case PersonType.Companion:
                friendAIList.Add(aiAgent);
                break;
            case PersonType.Enemy:
            case PersonType.Boss:
                enemyAIList.Add(aiAgent);
                break;
                /*bossAIList.Add(aiAgent);
                break;*/
            default:
                personAIList.Add(aiAgent);
                break;
        }
    }

    public void RemoveFromAIList(PersonType personType, NavMeshAgent aiAgent)
    {
        switch (personType)
        {
            case PersonType.Friend:
            case PersonType.Companion:
                friendAIList.Remove(aiAgent);
                break;
            case PersonType.Enemy:
            case PersonType.Boss:
                enemyAIList.Remove(aiAgent);
                break;
                /*bossAIList.Remove(aiAgent);
                break;*/
            default:
                personAIList.Remove(aiAgent);
                break;
        }
    }

    public bool AIListsContain(NavMeshAgent aiAgent)
    {
        if (friendAIList.Contains(aiAgent) || personAIList.Contains(aiAgent) || enemyAIList.Contains(aiAgent) /*|| bossAIList.Contains(aiAgent)*/)
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
            case PersonType.Companion:
                aiList = friendAIList;
                break;
            case PersonType.Enemy:
            case PersonType.Boss:
                aiList = enemyAIList;
                break;
                /*aiList = bossAIList;
                break;*/
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

    public void AgentCircleTarget(List<NavMeshAgent> aiList, NavMeshAgent aiAgent, Transform targetTransform, CircleType circleType) {
        int circleMultiplier = circleType == CircleType.FullCircle ? 2 : 1;
        int agentIndex = -1;
        float circleSpacing = 3f;

        agentIndex = aiList.IndexOf(aiAgent);

        if (agentIndex != -1) {
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
            case PersonType.Companion:
                aiList = friendAIList;
                break;
            case PersonType.Enemy:
            case PersonType.Boss:
                aiList = enemyAIList;
                break;
                /*aiList = bossAIList;
                break;*/
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
                targetTransform.position.x + RadiusAroundTarget * Mathf.Cos(circleMultiplier * Mathf.PI * i / aiList.Count),
                targetTransform.position.y,
                targetTransform.position.z + RadiusAroundTarget * Mathf.Sin(circleMultiplier * Mathf.PI * i / aiList.Count)
            );
            aiList[i].SetDestination(circleDestination);
        }
    }

    public /*void*/ Vector3 AgentRepositionAtDestination(NavMeshAgent aiAgent, Transform destinationTransform, PersonType personType, bool moveToDestination = true)
    {
        List<NavMeshAgent> aiList;
        int agentIndex = -1;

        switch (personType)
        {
            case PersonType.Friend:
            case PersonType.Companion:
                aiList = friendAIList;
                break;
            case PersonType.Enemy:
            case PersonType.Boss:
                aiList = enemyAIList;
                break;
                /*aiList = bossAIList;
                break;*/
            default:
                aiList = personAIList;
                break;
        }

        Vector3 reposition = Vector3.zero;
        agentIndex = aiList.IndexOf(aiAgent);
        if (agentIndex != -1)
        {
            reposition = CalculateLineUpPosition(aiList, agentIndex, destinationTransform);
            if(moveToDestination) aiAgent.SetDestination(reposition);
        }
        return reposition;
    }

    public int agentsPerRow = 5;
    public float lineUpSpacing = 10f;
    public Vector3 CalculateLineUpPosition(List<NavMeshAgent> aiList, int agentIndex, Transform destinationTransform) {
        int lineUpAgentsPerRow = /*5*/ agentsPerRow;
        int rowsToLineUp = Mathf.CeilToInt((float) aiList.Count / lineUpAgentsPerRow);

        List<Vector3> lineUpPositions = new List<Vector3>();
        Vector3 lineUpPosition = Vector3.zero;

        int startAgentIndex = 0;
        int endAgentIndex = aiList.Count - 1;
        int middleAgentIndex = (startAgentIndex + endAgentIndex) / 2;
        
        // change startAgentIndex, endAgentIndex and middleAgentIndex
        int halfStartIndex = 0;
        for(int row = 0; row < rowsToLineUp; row = row + 1) {
            int column = 0;

            int halfEndIndex = halfStartIndex + lineUpAgentsPerRow - 1 > endAgentIndex ? endAgentIndex : halfStartIndex + lineUpAgentsPerRow - 1;
            int halfMiddleIndex = halfStartIndex == endAgentIndex ? halfStartIndex : (halfStartIndex + halfEndIndex) / 2;
            // debug with rowStart, rowMiddle and rowEnd

            for (int agent = 0; agent < /*aiList.Count*/ lineUpAgentsPerRow; agent = agent + 1) {
                Vector3 newLineUpPosition = Vector3.zero;
                if (agentIndex == halfMiddleIndex /*middleAgentIndex*/) {
                    // center
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x,
                        destinationTransform.position.y,
                        destinationTransform.position.z - (row * lineUpSpacing)
                    );
                    
                    column = 0;
                } else if (agentIndex < halfMiddleIndex /*middleAgentIndex*/) {
                    // left (1st), change end
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x - ((agent + 1) * lineUpSpacing), // From middle as space factor. Math observation leads to use agent.
                        destinationTransform.position.y,
                        destinationTransform.position.z - (row * lineUpSpacing)
                    );
                    // halfEndIndex = halfMiddleIndex; // < <(end)
                    
                    column = agent + 1;
                } else if (agentIndex > halfMiddleIndex /*middleAgentIndex*/) {
                    // right (2nd), change start
                    newLineUpPosition = new Vector3(
                        destinationTransform.position.x - ((halfMiddleIndex /*middleAgentIndex*/ - agentIndex ) * lineUpSpacing), // From middle as space factor. Math observation leads to use agentIndex instead of agent.
                        destinationTransform.position.y,
                        destinationTransform.position.z - (row * lineUpSpacing)
                    );
                    // halfStartIndex = halfMiddleIndex + 1; // (start)> >
                    
                    column = halfMiddleIndex - agentIndex; // multiply with -1
                }
                Vector3 lineUpRotatePosition = (column * lineUpSpacing * destinationTransform.right) + (row * lineUpSpacing * destinationTransform.forward);
                newLineUpPosition = destinationTransform.position + lineUpRotatePosition;
                lineUpPositions.Add(newLineUpPosition);
            }

            halfStartIndex = halfStartIndex == endAgentIndex ? endAgentIndex : halfEndIndex + 1;
        }

        if (lineUpPositions.Count > agentIndex) {
            lineUpPosition = lineUpPositions[agentIndex];
        }

        return lineUpPosition;
    }

    void StartCombatAI()
    {
        if (CombatAILoopCoroutine != null) { // guard and restart cleanly so that the same coroutine won't run multiple times
            StopCoroutine(CombatAILoopCoroutine);
        }
        CombatAILoopCoroutine = StartCoroutine(SetCombatAILoopCoroutine(null));
    }

    public void SetupCombatAI()
    {
        Enemy []enemies = FindObjectsOfType<Enemy>();
        if(enemies.Length > 0) {
            foreach (Enemy enemy in enemies)
            {
                EnemyStruct enemyStruct = new EnemyStruct();
                enemyStruct.enemy = enemy;
                enemyStruct.available = true;
                enemyStructs.Add(enemyStruct);
            }

            StartCombatAI();
        }
    }

    public Enemy RandomEnemy()
    {
        List<int> randomLocationList = new List<int>();
        
        for (int i = 0; i < enemyStructs.Count; i = i + 1)
        {
            if (enemyStructs[i].available)
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
            if (enemyStructs[i].available && enemyStructs[i].enemy != excludingEnemy)
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

    public EnemyStruct GetCombatEnemy(Enemy enemy) {
        foreach(EnemyStruct enemyStruct in enemyStructs) {
            if(enemyStruct.enemy == enemy) {
                return enemyStruct;
            }
        }
        return default(EnemyStruct);
    }

    public void RemoveCombatEnemy(Enemy enemy)
    {
        int removeLocation = -1;
        foreach(EnemyStruct enemyStruct in enemyStructs)
        {
            removeLocation += 1;
            if (enemyStruct.enemy == enemy)
            {
                break;
            }
        }
        if(removeLocation > -1) enemyStructs.Remove(enemyStructs[removeLocation]);
    }
    
    public void AIListRegister(PersonType personType, NavMeshAgent navMeshAgent) {
        if(AIListsContain(navMeshAgent) == false) {
            AddToAIList(personType, navMeshAgent);
        }
    }

    public void AIListUnregister(PersonType personType, NavMeshAgent navMeshAgent) {
        if(AIListsContain(navMeshAgent) == true) {
            RemoveFromAIList(personType, navMeshAgent);
        }
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

        if (combatingEnemy.personAI.aiType == AIType.CombatAI)
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

    string textFieldInput = "";
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.fontSize = 30;

        if (SceneManager.instance.GetCurrentScene().name == "game") {
            if (GUI.Button(new Rect(30, 250, 350, 100), "Agents to Destination", guiStyle)) {
                foreach(Friend soldier in GameManager.instance.friendList) {
                    soldier.personAgent.isStopped = false;
                    soldier.personAgent.speed = 100f;
                    AgentRepositionAtDestination(soldier.personAgent, soldier.destination, PersonType.Friend);
                }

                foreach (Enemy enemy in GameManager.instance.enemyList) {
                    enemy.personAgent.isStopped = false;
                    enemy.personAgent.speed = 100f;
                    AgentRepositionAtDestination(enemy.personAgent, enemy.destination, PersonType.Enemy);
                }
            }
        }

        if (SceneManager.instance.GetCurrentScene().name == "lab")
        {
            if (GUI.Button(new Rect(30, 250, 250, 100), "Run CombatAI", guiStyle)) {
                SetupCombatAI();
            }
            
            GUI.Label(new Rect(30, 360, 360, 50), "Enter AI Agents per Row:", guiStyle);
            textFieldInput = GUI.TextField(new Rect(30, 410, 360, 50), textFieldInput, guiStyle);

            if (GUI.Button(new Rect(30, 470, 360, 100), "Agents to Destination", guiStyle)) {
                agentsPerRow = int.Parse(textFieldInput);
                foreach(Friend soldier in GameManager.instance.friendList) {
                    soldier.personAgent.isStopped = false;
                    soldier.personAgent.speed = 100f;
                    AgentRepositionAtDestination(soldier.personAgent, soldier.destination, PersonType.Friend);
                }

                foreach (Enemy enemy in GameManager.instance.enemyList) {
                    enemy.personAgent.isStopped = false;
                    enemy.personAgent.speed = 100f;
                    AgentRepositionAtDestination(enemy.personAgent, enemy.destination, PersonType.Enemy);
                }
            }
        }
    }
}