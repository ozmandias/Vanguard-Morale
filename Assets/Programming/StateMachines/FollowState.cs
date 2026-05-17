using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : StateMachineBehaviour {
    [SerializeField] Person mainPerson;
    CharacterInfo targetInfo;
    CombatManager targetCombat;
    float targetDistance;

    public float followDistance = 250f;
    public float circleDistance = 100f;
    public float attackDistance = 10f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;
        mainPerson.personAgent.isStopped = false;

        attackDistance = mainPerson.GetInfo().combatType == CombatType.Melee ? 10f : 50f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? (CharacterInfo) mainPerson.target.GetComponent<Vanguard>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Player>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).person.personAI.aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }

            targetCombat = mainPerson.target.GetComponent<CombatManager>();
            if(canAttackTarget) {
                targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                if(targetDistance <= followDistance && targetDistance > circleDistance && targetInfo.isDead == false) {
                    mainPerson.personAgent.destination = mainPerson.target.transform.position;
                }
                else if(targetDistance <= circleDistance && targetDistance > attackDistance && targetInfo.isDead == false) {
                    if(/*targetCombat.CirclingListContains(mainPerson.personAgent) == false &&*/ targetCombat.IsCirclingListFull() == false) {
                        // targetCombat.circlingList.Add(mainPerson.personAgent);
                        if(targetCombat.OnCirclingListRegister != null)
                            targetCombat.OnCirclingListRegister.Invoke(mainPerson.personAgent);
                    } else if(targetCombat.CirclingListContains(mainPerson.personAgent) == false && targetCombat.IsCirclingListFull()) {
                        mainPerson.SetTarget(null);
                        mainPerson.personState.stateMachineTargeting = false;
                        return;
                    }

                    if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        AIManager.instance.AgentCircleTarget(targetCombat.circlingList /*mainPerson.GetInfo().personType*/, mainPerson.personAgent, mainPerson.target.transform, CircleType.Semicircle);
                    }
                }
                else if(targetDistance <= attackDistance && targetInfo.isDead == false) {
                    mainPerson.personAgent.velocity = Vector3.zero;
                    mainPerson.personState.stateMachineAttacking = true;
                }
                else if(targetDistance > followDistance || targetInfo.isDead == true) {
                    /*if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        targetCombat.circlingList.Remove(mainPerson.personAgent);
                    }*/
                    if(targetCombat.OnCirclingListUnregister != null)
                        targetCombat.OnCirclingListUnregister.Invoke(mainPerson.personAgent);
                    mainPerson.SetTarget(null);
                    mainPerson.personState.stateMachineTargeting = false;
                }
            } else {
                /*if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                    targetCombat.circlingList.Remove(mainPerson.personAgent);
                }*/
                if(targetCombat.OnCirclingListUnregister != null)
                    targetCombat.OnCirclingListUnregister.Invoke(mainPerson.personAgent);
                mainPerson.SetTarget(null);
                mainPerson.personState.stateMachineTargeting = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}