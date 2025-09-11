using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    CombatManager targetCombat;
    float targetDistance;

    public float followDistance = 250f;
    public float circleDistance = 100f;
    public float nearDistance = 10f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        mainPerson.personAgent.isStopped = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? (Info) mainPerson.target.GetComponent<MasterKnight>().GetInfo() : (Info) mainPerson.target.GetComponent<Player>().GetInfo() : (Info) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }

            if(canAttackTarget) {
                targetCombat = mainPerson.target.GetComponent<CombatManager>();

                targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                if(targetDistance <= followDistance && targetDistance > circleDistance && targetInfo.isDead == false) {
                    mainPerson.personAgent.destination = mainPerson.target.transform.position;
                } else if(targetDistance <= circleDistance && targetDistance > nearDistance && targetInfo.isDead == false) {
                    if(targetCombat.CirclingListContains(mainPerson.personAgent) == false && targetCombat.IsCirclingListFull() == false) {
                        targetCombat.circlingList.Add(mainPerson.personAgent);
                    } else if(targetCombat.CirclingListContains(mainPerson.personAgent) == false && targetCombat.IsCirclingListFull()) {
                        mainPerson.SetTarget(null);
                        mainPerson.attackingTarget = false;
                        return;
                    }

                    if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        AIManager.instance.AgentCircleTarget(mainPerson.GetInfo().personType, mainPerson.personAgent, mainPerson.target.transform, CircleType.Semicircle);
                    }
                } else if(targetDistance <= nearDistance && targetInfo.isDead == false) {
                    mainPerson.personAgent.velocity = Vector3.zero;
                    mainPerson.nearTarget = true;
                } else if(targetDistance > followDistance || targetInfo.isDead == true) {
                    if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        targetCombat.circlingList.Remove(mainPerson.personAgent);
                    }
                    mainPerson.SetTarget(null);
                    mainPerson.attackingTarget = false;
                }
            } else {
                mainPerson.attackingTarget = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.GetInfo().isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }
}