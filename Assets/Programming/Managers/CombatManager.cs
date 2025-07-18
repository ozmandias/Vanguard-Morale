using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CombatManager : MonoBehaviour
{
    public List<NavMeshAgent> circlingList;
    public int maxCirclingEnemies = 3;

    [Space(10)]
    public AnimationManager animationManager;
    public Camera playerCamera;
    public Info characterInfo;
    public Collider combatCollider;
    public Vector3 checkDirection;
    public bool combatManagerTakesOver = false;
    public bool managingAttack = false;
    public Enemy currentTarget;
    public bool isAttackable = true;
    public int combatNumber = 0;
    public float enemyCheckRadius = 10f;
    public float enemyCheckMaxDistance = 30f;
    public float targetAttackOffset = 10f;
    public LayerMask layerMask;

    void Start()
    {
        characterInfo = GetComponent<Info>();
        if (characterInfo is MasterKnightInfo || characterInfo is PlayerInfo)
        {
            playerCamera = Camera.main;
            animationManager = GetComponent<AnimationManager>();
            combatCollider = characterInfo is MasterKnightInfo ? GameObject.FindWithTag("MasterKnightAttackCollider").GetComponent<Collider>() : GameObject.FindWithTag("PlayerAttackCollider").GetComponent<Collider>();
        }
    }

    void Update()
    {
        if (characterInfo is MasterKnightInfo || characterInfo is PlayerInfo)
        {
            CheckEnemies();
            AttackTarget();
        }
    }

    void CheckEnemies()
    {
        checkDirection = playerCamera.transform.forward * Input.GetAxisRaw("Vertical") + playerCamera.transform.right * Input.GetAxisRaw("Horizontal");
        checkDirection = checkDirection.normalized;

        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, enemyCheckRadius, checkDirection, out hitInfo, enemyCheckMaxDistance, layerMask))
        {
            if (hitInfo.collider.gameObject.GetComponent<Enemy>())
            {
                currentTarget = hitInfo.collider.gameObject.GetComponent<Enemy>().personCombat.isAttackable ? hitInfo.collider.gameObject.GetComponent<Enemy>() : null;
                managingAttack = true;
            }
        }
        else
        {
            currentTarget = null;
            managingAttack = false;
        }
    }

    void MoveTowardsTarget()
    {
        combatManagerTakesOver = true;
        transform.DOLookAt(currentTarget.transform.position, 0.2f);
        transform.DOMove(TargetOffset(targetAttackOffset), 0.65f);
        combatManagerTakesOver = false;
    }

    public Vector3 TargetOffset(float offset)
    {
        Vector3 targetPosition;
        targetPosition = currentTarget.transform.position;
        return Vector3.MoveTowards(targetPosition + currentTarget.transform.forward * offset, transform.position, 0.95f);
    }

    void AttackTarget()
    {
        if (currentTarget && Input.GetKeyDown(KeyCode.Mouse0))
        {
            MoveTowardsTarget();
            combatCollider.enabled = true;
            combatNumber += 1;
            combatNumber = combatNumber == 3 ? 1 : combatNumber;
            GetComponent<AnimationManager>().Play("Combat" + combatNumber);
        }
    }

    float counterTimer = 0;
    public void CounterAttack(float attackTimer)
    {
        counterTimer += Time.deltaTime;
        if (counterTimer < attackTimer / 2)
        {
            MoveTowardsTarget();
        }
    }

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
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, checkDirection);
        Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
        if(currentTarget) Gizmos.DrawSphere(currentTarget.transform.position, 0.5f);
    }
}