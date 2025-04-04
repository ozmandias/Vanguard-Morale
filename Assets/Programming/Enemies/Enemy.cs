using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Person {
    public override void Start() {
        base.Start();
        personDestination = GameObject.Find("EnemyDestination").transform;
        target = GameManager.instance.playerGameObject;

        GameManager.instance.enemyList.Add(this);
    }

    public override void Idle() {
        personAnimation.Play("Idle");
    }

    public override void Move() {
        personAnimation.Play("Move");
    }

    public override void Attack() {
        personAnimation.Play("Attack");
    }

    public override void Work() {
        personAnimation.Play("Work");
    }

    public override void Follow() {
        personAnimation.Play("Follow");
    }

    public override void FindTarget() {
        if(attackingTarget == false) {
            float targetPlayerDistance = 0;
            float targetSoldierDistance = 0;
            float targetPersonDistance = 0;
            GameObject targetPlayer = GameManager.instance.playerGameObject;
            GameObject targetSoldier = null;
            GameObject targetPerson = null;

            targetPlayerDistance = Vector3.Distance(GameManager.instance.playerGameObject.transform.position, transform.position);

            foreach(Person person in GameManager.instance.personList) {
                targetPersonDistance = Vector3.Distance(person.transform.position, transform.position);
                if(targetPersonDistance <= 300f) {
                    targetPerson = person.gameObject;
                    break;
                }
            }

            if(targetPlayerDistance < targetPersonDistance && targetPlayerDistance <= 300f) {
                SetTarget(targetPlayer);
            } else if(targetPersonDistance < targetPlayerDistance && targetPersonDistance <= 300f) {
                SetTarget(targetPerson);
            }
        }
    }

    public override void Hurt(int hurtAmount) {
        base.Hurt(hurtAmount);
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.enemyList.Remove(this);
    }

    public override void Resurrect() {
        base.Resurrect();
        GameManager.instance.enemyList.Add(this);
    }
}