using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Person {
    public delegate void SpecialAbilityDelegate();
    public SpecialAbilityDelegate OnSpecialAbility;

    public override void Start() {
        base.Start();
        
        personInfo.damage = 50;

        if(personInfo.personType == PersonType.Companion) {
            destination = GameManager.instance.friendDestination;
            GameManager.instance.companionList.Add(this);
            AIManager.instance.friendAIList.Add(personAgent);
        } else if(personInfo.personType == PersonType.Boss) {
            destination = GameManager.instance.enemyDestination;
            GameManager.instance.bossList.Add(this);
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
        if(personInfo.combatType == CombatType.Range) {
            if(attackNumberUpdate) {
                attackNumber += 1;
                attackNumber = attackNumber > 1 ? 0 : attackNumber;
                personAnimation.SetParameter("AttackNumber", (float) attackNumber);
                attackNumberUpdate = false;
                ChangeRaycastShooter();
            }
        }
    }

    public override void Hurt() {
        base.Hurt();
    }

    public override void Dead() {
        base.Dead();

        GameManager.instance.personList.Remove(this);
        AIManager.instance.personAIList.Remove(personAgent);
    }

    public override void Resurrect() {
        base.Resurrect();

        GameManager.instance.personList.Add(this);
        AIManager.instance.personAIList.Add(personAgent);
    }

    public override void FindTarget() {
        if(personInfo.personType == PersonType.Companion) {

        } else if(personInfo.personType == PersonType.Boss) {
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

                foreach(var soldier in GameManager.instance.friendList) {
                    if(!personAgent.Raycast(soldier.gameObject.transform.position, out personNavMeshHit) && soldier.GetInfo().isDead == false) {
                        targetSoldier = soldier.gameObject;
                        targetList.Add(targetSoldier);
                        break;
                    }
                }

                foreach(var person in GameManager.instance.personList) {
                    if(!personAgent.Raycast(person.gameObject.transform.position, out personNavMeshHit) && person.GetInfo().isDead == false) {
                        targetPerson = person.gameObject;
                        targetList.Add(targetPerson);
                        break;
                    }
                }

                foreach(var target in targetList) {
                    float targetDistance = Vector3.Distance(target.transform.position, transform.position);
                    if (targetDistance < nearestDistance) {
                        nearestDistance = targetDistance;
                        SetTarget(target);
                    }
                }

                targetList.Clear();
            }
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

    public virtual void SpecialAbility() {
        
    }
}