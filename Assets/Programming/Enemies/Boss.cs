using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Person
{
    public delegate void SpecialAbilityDelegate();
    public SpecialAbilityDelegate OnSpecialAbility;

    public override void Start() {
        base.Start();

        personInfo.personType = PersonType.Boss;

        destination = GameManager.instance.enemyDestination;

        GameManager.instance.bossList.Add(this);
        AIManager.instance.enemyAIList .Add(personAgent); /*bossAIList*/

        OnSpecialAbility += SpecialAbility;
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
        if(attackNumberUpdate) {
            attackNumber += 1;
            attackNumber = attackNumber > 2 ? 1 : attackNumber;
            personAnimation.SetParameter("AttackNumber", (float) attackNumber);
            attackNumberUpdate = false;
            base.Attack();
        }
    }

    public override void Hurt() {
        base.Hurt();
    }

    public override void Dead() {
        base.Dead();
    }

    public virtual void SpecialAbility() {}

    public override void FindTarget() {
        GameObject targetPlayer = GameManager.instance.playerGameObject;
        if(targetPlayer) {
            if(!personAgent.Raycast(targetPlayer.transform.position, out personNavMeshHit)) {
                SetTarget(targetPlayer);
            }
        }

        if(!target) {
            GameObject targetSoldier = null;
            GameObject targetPerson = null;
            List<GameObject> targetList = new List<GameObject>();
            float nearestDistance = float.MaxValue;

            foreach(Friend soldier in GameManager.instance.soldierList) {
                if(!personAgent.Raycast(soldier.gameObject.transform.position, out personNavMeshHit) && soldier.GetInfo().isDead == false) {
                    targetSoldier = soldier.gameObject;
                    targetList.Add(targetSoldier);
                    break;
                }
            }

            foreach(Person person in GameManager.instance.personList) {
                if(!personAgent.Raycast(person.gameObject.transform.position, out personNavMeshHit) && person.GetInfo().isDead == false) {
                    targetPerson = person.gameObject;
                    targetList.Add(targetPerson);
                    break;
                }
            }

            foreach(GameObject target in targetList) {
                float targetDistance = Vector3.Distance(target.transform.position, transform.position);
                if (targetDistance < nearestDistance) {
                    nearestDistance = targetDistance;
                    SetTarget(target);
                }
            }

            targetList.Clear();
        }
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
}