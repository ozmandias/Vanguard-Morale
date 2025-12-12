using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

public class CombatManager : MonoBehaviour {
    [Header("Combat Manager Settings")]
    public AnimationManager animationManager;
    public EffectManager effectManager;
    public List<NavMeshAgent> circlingList;
    public int maxCirclingEnemies = 3;
    public List<NavMeshAgent> combatingList;
    public int maxCombatingEnemies = 5;
    public Info characterInfo;
    public bool available = true;

    [Header("Player Combat Settings")/*Space(10)*/]
    public Camera playerCamera;
    public Collider combatCollider;
    public Vector3 checkDirection;
    public Enemy currentTarget;
    public bool managingMove = false;
    public bool managingAttack = false;
    public bool isCombating = false;
    public bool isCountering = false;
    public int combatNumber = 0;
    public int counterNumber = 0;
    public float targetCheckRadius = 12f;
    public float targetCheckMaxDistance = 30f;
    public float targetAttackOffset = 5f;
    public LayerMask layerMask;
    public PlayerMovementEvent OnPlayerMovement = new PlayerMovementEvent();
    public PlayerCombatEvent OnPlayerCombat = new PlayerCombatEvent();
    public PlayerCounterEvent OnPlayerCounter = new PlayerCounterEvent();

    [Header("Enemy Combat Settings")]
    Vector3 enemyMoveAroundDirection;
    public bool enemyIsNearPlayer = false;
    public bool enemyIsPreparingAttack = false;
    public bool enemyIsMoving = false;
    public bool enemyIsAttacking = false;
    public bool enemyIsRetreating = false;
    public bool enemyIsPlayerTarget = false;
    public bool enemyIsStunned = false;
    public bool enemyIsWaiting = true;
    public bool enemyIsAttackable = true;
    public bool counterAlert = false;
    Coroutine MoveAroundPlayerCoroutine;
    Coroutine AttackPlayerCoroutine;
    Coroutine RetreatFromPlayerCoroutine;
    Coroutine EnemyHurtCoroutine;
    public EnemyStopEvent OnEnemyStop = new EnemyStopEvent();
    public EnemyRetreatEvent OnEnemyRetreat = new EnemyRetreatEvent();
    public EnemyHurtEvent OnEnemyHurt = new EnemyHurtEvent();

    void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        effectManager = GetComponent<EffectManager>();
        characterInfo = gameObject.CompareTag("Player") ? gameObject.name == PlayerCharacter.MasterKnight.ToString() ? (Info)GetComponent<MasterKnight>().GetInfo() : (Info)GetComponent<Player>().GetInfo() : (Info)GetComponent<Person>().GetInfo();
        if (characterInfo is MasterKnightInfo || characterInfo is PlayerInfo)
        {
            playerCamera = Camera.main;
            combatCollider = characterInfo is MasterKnightInfo ? GameObject.FindWithTag("MasterKnightAttackCollider").GetComponent<Collider>() : GameObject.FindWithTag("PlayerAttackCollider").GetComponent<Collider>();
        }
        else if (characterInfo is PersonInfo && GameManager.instance.playerGameObject)
        {
            CombatManager playerCombat = GameManager.instance.playerGameObject.GetComponent<CombatManager>();
            playerCombat.OnPlayerMovement.AddListener((combatEnemy) => OnPlayerMovementEvent(combatEnemy));
            playerCombat.OnPlayerCombat.AddListener((combatEnemy) => OnPlayerCombatEvent(combatEnemy));
            playerCombat.OnPlayerCounter.AddListener((combatEnemy) => OnPlayerCounterEvent(combatEnemy));

            MoveAroundPlayerCoroutine = StartCoroutine(SetMoveAroundPlayerCoroutine());
            OnEnemyStop.AddListener((combatEnemy) => OnEnemyCombatStopEvent(combatEnemy));
            OnEnemyHurt.AddListener((combatEnemy) => OnEnemyHurtEvent(combatEnemy));
        }
    }

    void Update()
    {
        if (characterInfo is MasterKnightInfo || characterInfo is PlayerInfo)
        {
            CheckEnemies();
            AttackTarget();
            CounterTarget();
        }
        else if (characterInfo is PersonInfo && GameManager.instance.playerGameObject)
        {
            PersonInfo personInfo = characterInfo as PersonInfo;
            if (personInfo.personType == PersonType.Enemy && personInfo.aiType == AIType.CombatAI)
            {
                Vector3 playerCombatPosition = GameManager.instance.playerGameObject.transform.position;
                transform.LookAt(new Vector3(playerCombatPosition.x, transform.position.y, playerCombatPosition.z));
                MoveAroundPlayer(enemyMoveAroundDirection);
                CheckCounterAlert();
            }
        }
    }



    #region Player
    void CheckEnemies()
    {
        checkDirection = playerCamera.transform.forward * Input.GetAxisRaw("Vertical") + playerCamera.transform.right * Input.GetAxisRaw("Horizontal");
        checkDirection = checkDirection.normalized;

        if (isCombating)
        {
            checkDirection = Vector3.zero.normalized;
        }

        /*RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, targetCheckRadius, checkDirection, out hitInfo, targetCheckMaxDistance, layerMask))
        {
            if (hitInfo.collider.gameObject.GetComponent<Enemy>())
            {
                currentTarget = hitInfo.collider.gameObject.GetComponent<Enemy>().personCombat.enemyIsAttackable ? hitInfo.collider.gameObject.GetComponent<Enemy>() : null;
                managingAttack = true;
            }
        }*/

        Collider[] hits = Physics.OverlapSphere(transform.position, targetCheckRadius, layerMask);

        Enemy bestCandidate = null;
        float bestDot = 0.5f; // adjust: higher = narrower angle
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy.personCombat.enemyIsAttackable)
            {
                Vector3 toEnemy = (enemy.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(checkDirection, toEnemy);
                float dist = Vector3.Distance(transform.position, enemy.transform.position);

                if (dot > bestDot && dist <= targetCheckMaxDistance && dist < closestDistance)
                {
                    bestCandidate = enemy;
                    closestDistance = dist;
                }
            }
        }

        if (bestCandidate != null)
        {
            currentTarget = bestCandidate;
            managingAttack = true;
        }

        if (currentTarget && (Vector3.Distance(currentTarget.transform.position, transform.position) > targetCheckMaxDistance || currentTarget.personCombat.enemyIsAttackable == false))
        {
            currentTarget = null;
            managingAttack = false;
        }
    }

    void MoveTowardsTarget(string moveTowardsType)
    {
        managingMove = true;
        OnPlayerMovement.Invoke(currentTarget);
        targetAttackOffset = moveTowardsType == "combat" ? 5 : moveTowardsType == "counter" ? 7 : 0;
        transform.DOLookAt(currentTarget.transform.position, 0.5f /*1f*/ /*0.2f*/);
        transform.DOMove(TargetOffset(targetAttackOffset), 0.5f/*0.65f*/);
    }

    public Vector3 TargetOffset(float offset)
    {
        Vector3 targetPosition;
        targetPosition = currentTarget.transform.position;
        return Vector3.MoveTowards(targetPosition + currentTarget.transform.forward * offset, transform.position, 0.95f);
    }

    void AttackTarget()
    {
        if (currentTarget && Input.GetKeyDown(KeyCode.Mouse0) && isCombating == false)
        {
            isCombating = true;
            MoveTowardsTarget("combat");
            combatNumber += 1;
            combatNumber = combatNumber == 3 ? 1 : combatNumber;
            animationManager.Play("Combat" + combatNumber);

            if(currentTarget.GetInfo().aiType == AIType.StateMachine) {
                currentTarget.GetComponent<AIChanger>().OnChangeAIDelegate("combatAI");
            }
        }
    }

    public void CounterTarget()
    {
        if (NearestEnemyToCounter() && Input.GetKeyDown(KeyCode.E) && isCombating == false && Vector3.Distance(transform.position, NearestEnemyToCounter().transform.position) < 10)
        {
            isCombating = true;
            currentTarget = NearestEnemyToCounter();
            MoveTowardsTarget("counter");
            counterNumber += 1;
            counterNumber = counterNumber == 3 ? 1 : counterNumber;
            animationManager.Play("Counter" + counterNumber);
        }
    }

    public Enemy NearestEnemyToCounter()
    {
        Enemy nearestEnemy = null;
        float minDistance = 100f;
        int nearestLocation = -1;

        if (AIManager.instance && AIManager.instance.enemyStructs.Count > 0)
        {
            for (int i = 0; i < AIManager.instance.enemyStructs.Count; i = i + 1)
            {
                Enemy currentCheckEnemy = AIManager.instance.enemyStructs[i].enemy;
                if (currentCheckEnemy.GetComponent<CombatManager>()/*personCombat*/.enemyIsPreparingAttack)
                {
                    float currentCheckDistance = Vector3.Distance(transform.position, currentCheckEnemy.transform.position);
                    if (currentCheckDistance < minDistance)
                    {
                        minDistance = currentCheckDistance;
                        nearestLocation = i;
                    }
                }
            }
        }

        if (nearestLocation > -1) nearestEnemy = AIManager.instance.enemyStructs[nearestLocation].enemy;
        return nearestEnemy;
    }
    #endregion



    #region Enemy
    public void MoveAroundPlayer(Vector3 direction)
    {
        float moveSpeed = /*1*/ 14;

        if (direction == Vector3.forward) { moveSpeed = /*15*/ 14; animationManager.SetParameter("VerticalMovement", 1f); /*animationManager.PlayWithParameter("Move", "Velocity", moveSpeed);*/ }
        if (direction == Vector3.right || direction == Vector3.left) { moveSpeed = moveSpeed / 2 /*1.5f*/; animationManager.SetParameter("HorizontalMovement", direction == Vector3.right ? 1f : -1f); /*animationManager.Play(direction == Vector3.right ? "StrafeRight" : "StrafeLeft");*/ }
        if (direction == Vector3.back) { moveSpeed = 12; animationManager.SetParameter("VerticalMovement", -1f); /*animationManager.Play("Retreat");*/ }

        if (enemyIsAttacking == false && enemyIsStunned == false) animationManager.Play("CombatMove");

        if (enemyIsMoving == false || enemyIsStunned == true) return;

        Vector3 moveDirection = (GameManager.instance.playerGameObject.transform.position - transform.position).normalized;
        Vector3 sideDirection = Quaternion.AngleAxis(90, Vector3.up) * moveDirection;

        Vector3 finalDirection = Vector3.zero;
        if (direction == Vector3.forward) finalDirection = moveDirection;
        if (direction == Vector3.right || direction == Vector3.left) finalDirection = sideDirection * direction.normalized.x;
        if (direction == Vector3.back) finalDirection = -transform.forward;
        finalDirection = finalDirection * moveSpeed * Time.deltaTime;

        // move
        transform.position += finalDirection;

        if (enemyIsPreparingAttack == false) return;

        // attack
        if (Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position) < 10 /*2*/)
        {
            enemyIsNearPlayer = true;
            StopAroundPlayer();

            // check counter
            CombatManager playerCombat = GameManager.instance.playerGameObject.GetComponent<CombatManager>();
            if (playerCombat.isCombating == false && playerCombat.isCountering == false)
                AttackPlayer();
            else
                PrepareAttackPlayer(false);
        }
    }

    public void StopAroundPlayer()
    {
        enemyIsMoving = false;
        enemyIsAttacking = false;
        if (enemyIsRetreating) enemyIsRetreating = false;
        enemyMoveAroundDirection = Vector3.zero;
        animationManager.SetParameter("HorizontalMovement", 0.0f); animationManager.SetParameter("VerticalMovement", 0.0f); // animationManager.Play("CombatIdle");
        transform.position += enemyMoveAroundDirection;
    }

    public void SetAttackPlayer()
    {
        enemyIsWaiting = false;
        AttackPlayerCoroutine = StartCoroutine(SetAttackPlayerCoroutine());
    }

    public void PrepareAttackPlayer(bool status)
    {
        enemyIsPreparingAttack = status;
        if (enemyIsPreparingAttack == true)
        {
            effectManager.StartEffect("counter");
        }
        else
        {
            StopAroundPlayer();
            effectManager.StopEffect("counter");
        }
    }

    public void AttackPlayer()
    {
        enemyIsAttacking = true;
        transform.DOMove(transform.position + (transform.forward / 1), 0.5f);
        animationManager.Play("Combat");
    }

    public void SetRetreatFromPlayer()
    {
        StopEnemyCoroutines();

        RetreatFromPlayerCoroutine = StartCoroutine(SetRetreatFromPlayerCoroutine());
    }

    public void GetHurtByPlayer()
    {

    }

    public void StopEnemyCoroutines()
    {
        PrepareAttackPlayer(false);

        if (enemyIsRetreating && RetreatFromPlayerCoroutine != null) StopCoroutine(RetreatFromPlayerCoroutine);
        if (AttackPlayerCoroutine != null) StopCoroutine(AttackPlayerCoroutine);
        // if (EnemyHurtCoroutine != null) StopCoroutine(EnemyHurtCoroutine);
        if (MoveAroundPlayerCoroutine != null) StopCoroutine(MoveAroundPlayerCoroutine);
    }

    void CheckCounterAlert()
    {
        if (Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position) < 20)
        {
            counterAlert = true;
        }
        else
        {
            counterAlert = false;
        }
    }

    void OnPlayerMovementEvent(Enemy enemyTarget)
    {
        if (enemyTarget == this.GetComponent<Enemy>())
        {
            StopEnemyCoroutines();
            enemyIsPlayerTarget = true;
            PrepareAttackPlayer(false);
            StopAroundPlayer();
        }
    }

    void OnPlayerCombatEvent(Enemy enemyTarget)
    {
        if (enemyTarget == this.GetComponent<Enemy>())
        {
            CombatManager playerCombat = GameManager.instance.playerGameObject.GetComponent<CombatManager>();
            playerCombat.currentTarget = null;
            playerCombat.managingAttack = false;
            playerCombat.isCombating = false;

            // OnEnemyHurt.Invoke(enemyTarget);
            StopEnemyCoroutines();
            // enemyIsStunned = true;
            EnemyHurtCoroutine = StartCoroutine(SetEnemyHurtCoroutine());
            enemyIsPlayerTarget = false;
            enemyTarget.GetInfo().CombatReduceHealth(playerCombat.characterInfo/*.damage*/); // <- after this line, set Enemy's availiability to false on Enemy's death
            if (characterInfo.isDead)
            {
                available = false;
                enemyIsAttackable = false;
                AIManager.instance.SetEnemyAvailable(enemyTarget, false);
                return;
            }
            animationManager.Play("CombatHurt");
            StopAroundPlayer();
        }
    }

    void OnPlayerCounterEvent(Enemy enemyTarget)
    {
        if (enemyTarget == this.GetComponent<Enemy>())
        {
            CombatManager playerCombat = GameManager.instance.playerGameObject.GetComponent<CombatManager>();

            StopEnemyCoroutines();
            enemyIsStunned = true;
            enemyIsPlayerTarget = false;
            animationManager.Play("CounterHurt" + playerCombat.counterNumber);
            StopAroundPlayer();
        }
    }

    void OnEnemyHurtEvent(Enemy enemyTarget)
    {
        if (enemyTarget == this.GetComponent<Enemy>())
        {
        }
    }

    void OnEnemyCombatStopEvent(Enemy enemy) {
        if(enemy == this.GetComponent<Enemy>()) {
            this.GetComponent<AIChanger>().OnChangeAIDelegate("stateMachine");
        }
    }
    #endregion



    #region CombatManager
    public bool IsCirclingListFull()
    {
        bool fullStatus = circlingList.Count == maxCirclingEnemies ? true : false;
        return fullStatus;
    }

    public bool CirclingListContains(NavMeshAgent circlingAgent)
    {
        bool containStatus = circlingList.Contains(circlingAgent);
        return containStatus;
    }

    public bool IsCombatingListFull() {
        bool fullStatus = combatingList.Count == maxCombatingEnemies ? true : false;
        return fullStatus;
    }

    public bool CombatingListContains(NavMeshAgent combatingAgent) {
        bool containStatus = combatingList.Contains(combatingAgent);
        return containStatus;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, checkDirection);
        Gizmos.DrawWireSphere(transform.position, targetCheckRadius);
        if (currentTarget) Gizmos.DrawSphere(currentTarget.transform.position, 0.5f);
    }
    #endregion



    IEnumerator SetMoveAroundPlayerCoroutine()
    {
        if (available == false) yield break;

        yield return new WaitUntil(() => enemyIsWaiting == true);

        int moveRandom = Random.Range(0, 2);
        if (moveRandom == 1)
        {
            int directionRandom = Random.Range(0, 2);
            enemyMoveAroundDirection = directionRandom == 1 ? Vector3.right : Vector3.left;
            enemyIsMoving = true;
            enemyIsAttacking = false;
        }
        else
        {
            StopAroundPlayer();
        }

        yield return new WaitForSeconds(1);

        MoveAroundPlayerCoroutine = StartCoroutine(SetMoveAroundPlayerCoroutine());
    }

    IEnumerator SetAttackPlayerCoroutine()
    {
        if (available == false) yield break;

        PrepareAttackPlayer(true);
        yield return new WaitForSeconds(0.2f);
        enemyMoveAroundDirection = Vector3.forward;
        enemyIsMoving = true;
    }

    IEnumerator SetRetreatFromPlayerCoroutine()
    {
        if (available == false) yield break;

        yield return new WaitForSeconds(0.5f /*1.4f*/);

        OnEnemyRetreat.Invoke(GetComponent<Enemy>());
        enemyIsRetreating = true;
        enemyMoveAroundDirection = Vector3.back;
        enemyIsMoving = true;
        enemyIsAttacking = false;

        yield return new WaitUntil(() => Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position) > 20 /*4*/);

        enemyIsNearPlayer = false;
        enemyIsRetreating = false;
        StopAroundPlayer();

        enemyIsWaiting = true;
        MoveAroundPlayerCoroutine = StartCoroutine(SetMoveAroundPlayerCoroutine());
    }

    IEnumerator SetEnemyHurtCoroutine()
    {
        enemyIsStunned = true;
        yield return new WaitForSeconds(0.5f);
        enemyIsStunned = false;
    }
}