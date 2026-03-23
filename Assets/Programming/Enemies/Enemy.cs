using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Person {
    public override void Start()
    {
        base.Start();
        
        personInfo.personType = PersonType.Enemy;

        // target = GameManager.instance.playerGameObject;
        destination = GameManager.instance.enemyDestination /*GameObject.Find("EnemyDestination").transform*/;

        GameManager.instance.enemyList.Add(this);
        AIManager.instance.enemyAIList.Add(personAgent);
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

    public override void Dead()
    {
        base.Dead();

        GameManager.instance.enemyList.Remove(this);
        AIManager.instance.enemyAIList.Remove(personAgent);
        AIManager.instance.RemoveCombatEnemy(this);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.enemyList.Add(this);
        AIManager.instance.enemyAIList.Add(personAgent);
    }

    public override void FindTarget() {
        /*float targetPlayerDistance = 0;
        float targetSoldierDistance = 0;
        float targetPersonDistance = 0;*/
        GameObject targetPlayer = GameManager.instance.playerGameObject;
        GameObject targetSoldier = null;
        GameObject targetPerson = null;
        List<GameObject> targetList = new List<GameObject>();
        float nearestDistance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();

        if(targetPlayer) {
            CombatManager playerCombat = targetPlayer.GetComponent<CombatManager>();
            if(playerCombat.IsCirclingListFull() == false && playerCombat.IsCombatingListFull() == false) {
                if(!personAgent.Raycast(targetPlayer.transform.position, out personNavMeshHit) && Vector3.Distance(targetPlayer.transform.position, transform.position) < 250f) {
                    // targetPlayerDistance = Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position);
                    targetList.Add(targetPlayer);
                } else {
                    // targetPlayerDistance = 0;
                    targetPlayer = null;
                }
            }
        }

        foreach(Friend soldier in GameManager.instance.soldierList) {
            CombatManager soldierCombat = soldier.GetComponent<CombatManager>();
            if(soldier.GetInfo().isDead == false && soldierCombat.IsCirclingListFull() == false && soldier.personAI.aiType == AIType.StateMachine) {
                /*targetSoldierDistance = Vector3.Distance(soldier.transform.position, transform.position);
                if(targetSoldierDistance <= 300f) {
                    targetSoldier = soldier.gameObject;
                    break;
                }*/

                if(!personAgent.Raycast(soldier.transform.position, out personNavMeshHit)) {
                    targetSoldier = soldier.gameObject;
                    targetList.Add(targetSoldier);
                    break;
                }
            }
        }

        foreach(Person person in GameManager.instance.personList) {
            CombatManager personCombat = person.GetComponent<CombatManager>();
            if(person.GetInfo().isDead == false && personCombat.IsCirclingListFull() == false && person.personAI.aiType == AIType.StateMachine) {
                /*targetPersonDistance = Vector3.Distance(person.transform.position, transform.position);
                if(targetPersonDistance <= 300f) {
                    targetPerson = person.gameObject;
                    break;
                }*/

                if(!personAgent.Raycast(person.transform.position, out personNavMeshHit)) {
                    targetPerson = person.gameObject;
                    targetList.Add(targetPerson);
                    break;
                }
            }
        }

        /*if(((targetPlayerDistance < targetSoldierDistance && targetSoldierDistance != 0) || (targetPlayerDistance > targetSoldierDistance && targetSoldierDistance == 0)) && ((targetPlayerDistance < targetPersonDistance && targetPersonDistance != 0) || (targetPlayerDistance > targetPersonDistance && targetPersonDistance == 0))) {
            SetTarget(targetPlayer);
        } else if((targetSoldierDistance < targetPlayerDistance && targetPlayerDistance != 0) || (targetSoldierDistance > targetPlayerDistance && targetPlayerDistance == 0) && ((targetSoldierDistance < targetPersonDistance && targetPersonDistance != 0) || (targetSoldierDistance > targetPersonDistance && targetPersonDistance == 0))) {
            SetTarget(targetSoldier);
        } else if(((targetPersonDistance < targetPlayerDistance && targetPlayerDistance != 0) || (targetPersonDistance > targetPlayerDistance && targetPlayerDistance == 0)) && ((targetPersonDistance < targetSoldierDistance && targetSoldierDistance != 0) || (targetPersonDistance > targetSoldierDistance && targetSoldierDistance == 0))) {
            SetTarget(targetPerson);
        }*/

        foreach(GameObject target in targetList) {
            if(NavMesh.CalculatePath(transform.position, target.transform.position, personAgent.areaMask, path)) {
                float targetDistance = Vector3.Distance(transform.position, path.corners[0]);
                for(int i = 1; i < path.corners.Length; i = i + 1) {
                    targetDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                if (targetDistance < nearestDistance)
                {
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

        if(otherCollider.gameObject.CompareTag("SoldierAttackCollider") || otherCollider.gameObject.CompareTag("PersonAttackCollider")) {
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