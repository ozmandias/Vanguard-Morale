using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Friend : Person {
    public override void Start() {
        base.Start();

        personInfo.personType = PersonType.Friend;

        destination = GameManager.instance.soldierDestination /*GameObject.Find("SoldierDestination").transform*/;

        GameManager.instance.soldierList.Add(this);
        AIManager.instance.soldierAIList.Add(personAgent);
    }

    public override void Idle() {
        base.Idle();
    }

    public override void Move() {
        base.Move();
    }

    public override void Work() {
        base.Work();
    }

    public override void Follow() {
        base.Follow();
    }

    public override void Attack() {
        base.Attack();
    }

    public override void Hurt() {
        base.Hurt();
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.soldierList.Remove(this);
        AIManager.instance.soldierAIList.Remove(personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.soldierList.Add(this);
        AIManager.instance.soldierAIList.Add(personAgent);
    }

    public override void FindTarget() {
        float targetEnemyDistance = 0;
        GameObject targetEnemy = null;
        GameObject targetBoss = null;
        List<GameObject> targetList = new List<GameObject>();
        float nearestDistance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();

        foreach(Enemy enemy in GameManager.instance.enemyList) {
            CombatManager enemyCombat = enemy.GetComponent<CombatManager>();
            if(enemy.GetInfo().isDead == false && enemyCombat.IsCirclingListFull() == false && enemy.personAI.aiType == AIType.StateMachine) {
                /*targetEnemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
                if(targetEnemyDistance <= 300f) {
                    targetEnemy = enemy.gameObject;
                    break;
                }*/

                if(!personAgent.Raycast(enemy.transform.position, out personNavMeshHit)) {
                    targetEnemy = enemy.gameObject;
                    targetList.Add(targetEnemy);
                    break;
                }
            }
        }

        /*if(targetEnemyDistance != 0) {
            SetTarget(targetEnemy);
        }*/

        /*if(targetEnemy) {
            SetTarget(targetEnemy);
        }*/

        foreach(Boss boss in GameManager.instance.bossList) {
            CombatManager bossCombat = boss.GetComponent<CombatManager>();
            if(boss.GetInfo().isDead == false && bossCombat.IsCirclingListFull() == false && boss.personAI.aiType == AIType.StateMachine) {
                if(!personAgent.Raycast(boss.transform.position, out personNavMeshHit)) {
                    targetBoss = boss.gameObject;
                    targetList.Add(targetBoss);
                    break;
                }
            }
        }

        foreach(GameObject target in targetList) {
            if(NavMesh.CalculatePath(transform.position, target.transform.position, personAgent.areaMask, path)) {
                float targetDistance = Vector3.Distance(transform.position, path.corners[0]);
                for(int i = 1; i < path.corners.Length; i = i + 1) {
                    targetDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                if(targetDistance < nearestDistance) {
                    nearestDistance = targetDistance;
                    SetTarget(target);
                    break;
                }
            }
        }
        targetList.Clear();
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("EnemyAttackCollider")) {
            Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
            if(personInfo.isDead == false && attackCharacterInfo.isDead == false) {
                personAnimation.SetParameter("HurtAmount", attackCharacterInfo.damage);
                personAnimation.SetParameter("ReduceHealth", true);
                personAnimation.mainAnimator.GetBehaviour<HurtState>().attackerInfo = attackCharacterInfo;
                hurtFrames = 0;
                isHurt = true;

                int changeTargetRandom = Random.Range(0, 10);
                if(personState.stateMachineTargeting == false || (personState.stateMachineTargeting && changeTargetRandom >= 5)) {
                    if(target) {
                        CombatManager currentTargetCombat = target.GetComponent<CombatManager>();
                        if(currentTargetCombat.CirclingListContains(personAgent)) {
                            currentTargetCombat.circlingList.Remove(personAgent);
                        }
                    }
                    SetTarget(attackCharacterInfo.owner);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}