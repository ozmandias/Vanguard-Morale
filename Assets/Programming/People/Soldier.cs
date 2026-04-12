using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Person {
    public override void Start() {
        base.Start();

        // set destination and add to lists
        if(personInfo.personType == PersonType.Friend) {
            destination = GameManager.instance.friendDestination;
            GameManager.instance.friendList.Add(this);
            AIManager.instance.friendAIList.Add(personAgent);
        } else if(personInfo.personType == PersonType.Enemy) {
            destination = GameManager.instance.enemyDestination;
            GameManager.instance.enemyList.Add(this);
            AIManager.instance.enemyAIList.Add(personAgent);
        }
    }

    public override void Update() {
        base.Update();
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

        if(personInfo.personType == PersonType.Friend) {
            GameManager.instance.friendList.Remove(this);
            AIManager.instance.friendAIList.Remove(personAgent);
        } else if(personInfo.personType == PersonType.Enemy) {
            GameManager.instance.enemyList.Remove(this);
            AIManager.instance.enemyAIList.Remove(personAgent);
        }
    }

    public override void Resurrect() {
        base.Resurrect();

        if(personInfo.personType == PersonType.Friend) {
            GameManager.instance.friendList.Add(this);
            AIManager.instance.friendAIList.Add(personAgent);
        } else if(personInfo.personType == PersonType.Enemy) {
            GameManager.instance.enemyList.Add(this);
            AIManager.instance.enemyAIList.Add(personAgent);
        }
    }

    public override void FindTarget() {
        List<GameObject> targetList = new List<GameObject>();
        float nearestDistance = float.MaxValue;
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        if(personInfo.personType == PersonType.Friend) {
            GameObject targetEnemy = null;
            GameObject targetBoss = null;

            foreach(var enemy in GameManager.instance.enemyList) {
                CombatManager enemyCombat = enemy.GetComponent<CombatManager>();
                if(enemy.GetInfo().isDead == false && enemyCombat.IsCirclingListFull() == false && enemy.personAI.aiType == AIType.StateMachine) {
                    if(!personAgent.Raycast(enemy.transform.position, out personNavMeshHit)) {
                        targetEnemy = enemy.gameObject;
                        targetList.Add(targetEnemy);
                        break;
                    }
                }
            }

            foreach(var boss in GameManager.instance.bossList) {
                CombatManager bossCombat = boss.GetComponent<CombatManager>();
                if(boss.GetInfo().isDead == false && bossCombat.IsCirclingListFull() == false && boss.personAI.aiType == AIType.StateMachine) {
                    if(!personAgent.Raycast(boss.transform.position, out personNavMeshHit)) {
                        targetBoss = boss.gameObject;
                        targetList.Add(targetBoss);
                        break;
                    }
                }
            }

            foreach(var target in targetList) {
                if(UnityEngine.AI.NavMesh.CalculatePath(transform.position, target.transform.position, personAgent.areaMask, path)) {
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
        } else if(personInfo.personType == PersonType.Enemy) {
            GameObject targetPlayer = GameManager.instance.playerGameObject;
            GameObject targetSoldier = null;
            GameObject targetPerson = null;

            if(targetPlayer) {
                CombatManager playerCombat = targetPlayer.GetComponent<CombatManager>();
                if(playerCombat.IsCirclingListFull() == false && playerCombat.IsCombatingListFull() == false) {
                    if(!personAgent.Raycast(targetPlayer.transform.position, out personNavMeshHit) && Vector3.Distance(targetPlayer.transform.position, transform.position) < 250f) {
                        targetList.Add(targetPlayer);
                    } else {
                        targetPlayer = null;
                    }
                }
            }

            foreach(var soldier in GameManager.instance.friendList) {
                CombatManager soldierCombat = soldier.GetComponent<CombatManager>();
                if(soldier.GetInfo().isDead == false && soldierCombat.IsCirclingListFull() == false && soldier.personAI.aiType == AIType.StateMachine) {
                    if(!personAgent.Raycast(soldier.transform.position, out personNavMeshHit)) {
                        targetSoldier = soldier.gameObject;
                        targetList.Add(targetSoldier);
                        break;
                    }
                }
            }

            foreach(var person in GameManager.instance.personList) {
                CombatManager personCombat = person.GetComponent<CombatManager>();
                if(person.GetInfo().isDead == false && personCombat.IsCirclingListFull() == false && person.personAI.aiType == AIType.StateMachine) {
                    if(!personAgent.Raycast(person.transform.position, out personNavMeshHit)) {
                        targetPerson = person.gameObject;
                        targetList.Add(targetPerson);
                        break;
                    }
                }
            }

            foreach(var target in targetList) {
                if(UnityEngine.AI.NavMesh.CalculatePath(transform.position, target.transform.position, personAgent.areaMask, path)) {
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
    }

    public override bool ShouldFindTarget() {
        return true;
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("SoldierAttackCollider")) {
            CharacterInfo attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Item>().GetOwnerInfo();
            if(personInfo.isDead == false && attackCharacterInfo.isDead == false){
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