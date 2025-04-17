using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float targetDistance;
    public bool nearTarget = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        mainPerson.personAgent.isStopped = false;

        if(mainPerson.target) {
            mainPerson.attackingTarget = true;
            targetInfo = mainPerson.target.GetComponent<Info>();
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance > 10f) {
                nearTarget = false;
                mainPerson.personAgent.destination = mainPerson.target.transform.position;
            } else {
                nearTarget = true;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance <= 250f && targetDistance > 100f && targetInfo.isDead == false && nearTarget == false) {
                mainPerson.personAgent.destination = mainPerson.target.transform.position;
            } else if(targetDistance <= 100f && targetDistance > 10f && targetInfo.isDead == false && nearTarget == false) {
                CombatManager targetCombat = mainPerson.target.GetComponent<CombatManager>();

                if(targetCombat.IsCirclingListFull() == false && targetCombat.CirclingListContains(mainPerson.personAgent) == false && targetInfo.isDead == false) {
                    targetCombat.circlingList.Add(mainPerson.personAgent);
                }
                
                if(targetCombat.CirclingListContains(mainPerson.personAgent)){
                    AIManager.instance.AgentCircleTarget(mainPerson.personInfo.personType, mainPerson.personAgent, mainPerson.target.transform, CircleType.Semicircle);
                } else if(targetCombat.IsCirclingListFull()) {
                    mainPerson.ChangeState(StateMachine.Wait);
                }
            } else if(targetDistance <= 10f && targetInfo.isDead == false) {
                nearTarget = true;
                mainPerson.ChangeState(StateMachine.Attack);
            } else {
                nearTarget = false;
            }

            if(targetDistance > 250f || targetInfo.isDead == true) {
                mainPerson.attackingTarget = false;
                mainPerson.ChangeState(StateMachine.Move);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.personInfo.isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }
}