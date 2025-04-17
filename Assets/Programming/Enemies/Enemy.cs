using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Person {
    public override void Start() {
        base.Start();

        personDestination = GameManager.instance.enemyDestination /*GameObject.Find("EnemyDestination").transform*/;
        target = GameManager.instance.playerGameObject;

        GameManager.instance.enemyList.Add(this);
        AIManager.instance.enemyAIList.Add(personAgent);
    }

    public override void Idle() {
        personAnimation.Play("Idle");
    }

    public override void Move() {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Move");
    }

    public override void Work() {
        personAnimation.Play("Work");
    }

    public override void Follow() {
        personAnimation.SetParameter("Velocity", personAgent.velocity.magnitude);
        personAnimation.Play("Follow");
    }

    public override void Attack() {
        personAnimation.Play("Attack");
    }

    public override void Wait() {
        personAnimation.Play("Wait");
    }

    public override void FindTarget() {
        float targetPlayerDistance = 0;
        float targetSoldierDistance = 0;
        float targetPersonDistance = 0;
        GameObject targetPlayer = GameManager.instance.playerGameObject;
        GameObject targetSoldier = null;
        GameObject targetPerson = null;

        CombatManager playerCombat = targetPlayer.GetComponent<CombatManager>();
        if(playerCombat.IsCirclingListFull() == false) {
            targetPlayerDistance = Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position);
        } else {
            targetPlayerDistance = 0;
            targetPlayer = null;
        }

        foreach(Friend soldier in GameManager.instance.soldierList) {
            CombatManager soldierCombat = soldier.GetComponent<CombatManager>();
            if(soldier.personInfo.isDead == false && soldierCombat.IsCirclingListFull() == false) {
                targetSoldierDistance = Vector3.Distance(soldier.transform.position, transform.position);
                if(targetSoldierDistance <= 300f) {
                    targetSoldier = soldier.gameObject;
                    break;
                }
            }
        }

        foreach(Person person in GameManager.instance.personList) {
            CombatManager personCombat = person.GetComponent<CombatManager>();
            if(person.personInfo.isDead == false && personCombat.IsCirclingListFull() == false) {
                targetPersonDistance = Vector3.Distance(person.transform.position, transform.position);
                if(targetPersonDistance <= 300f) {
                    targetPerson = person.gameObject;
                    break;
                }
            }
        }

        if(((targetPlayerDistance < targetSoldierDistance && targetSoldierDistance != 0) || (targetPlayerDistance > targetSoldierDistance && targetSoldierDistance == 0)) && ((targetPlayerDistance < targetPersonDistance && targetPersonDistance != 0) || (targetPlayerDistance > targetPersonDistance && targetPersonDistance == 0))) {
            SetTarget(targetPlayer);
        } else if((targetSoldierDistance < targetPlayerDistance && targetPlayerDistance != 0) || (targetSoldierDistance > targetPlayerDistance && targetPlayerDistance == 0) && ((targetSoldierDistance < targetPersonDistance && targetPersonDistance != 0) || (targetSoldierDistance > targetPersonDistance && targetPersonDistance == 0))) {
            SetTarget(targetSoldier);
        } else if(((targetPersonDistance < targetPlayerDistance && targetPlayerDistance != 0) || (targetPersonDistance > targetPlayerDistance && targetPlayerDistance == 0)) && ((targetPersonDistance < targetSoldierDistance && targetSoldierDistance != 0) || (targetPersonDistance > targetSoldierDistance && targetSoldierDistance == 0))) {
            SetTarget(targetPerson);
        }
    }

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.enemyList.Remove(this);
        AIManager.instance.enemyAIList.Remove(personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.enemyList.Add(this);
        AIManager.instance.enemyAIList.Add(personAgent);
    }

    public override void OnTriggerEnter(Collider otherCollider) {
        base.OnTriggerEnter(otherCollider);

        if(otherCollider.gameObject.CompareTag("SoldierAttackCollider") || otherCollider.gameObject.CompareTag("PersonAttackCollider")) {
            if(/*Time.time > nextHurtTime &&*/ personInfo.isDead == false) {
                // nextHurtTime = Time.time + hitRate;

                Info attackCharacterInfo = otherCollider.gameObject.GetComponentInParent<Info>();
                Hurt(attackCharacterInfo.damage);
                int attackBackRandom = Random.Range(0, 10);
                if(attackingTarget == false || attackBackRandom >= 5) {
                    SetTarget(attackCharacterInfo.gameObject);
                }
            }
        }
    }
    public override void OnTriggerExit(Collider otherCollider) {
        base.OnTriggerExit(otherCollider);
    }
}