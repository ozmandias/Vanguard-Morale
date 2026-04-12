using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour {
    [SerializeField] Person mainPerson;
    CharacterInfo targetInfo;
    float destinationDistance;
    float targetDistance;

    public float reachDistance = 0;
    public float followDistance = 250f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;
        mainPerson.personAgent.isStopped = true;

        reachDistance = mainPerson.personAgent.stoppingDistance;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? (CharacterInfo) mainPerson.target.GetComponent<Vanguard>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Player>().GetInfo() : (CharacterInfo) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).person.personAI.aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                    return;
                }
            }
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance <= followDistance && targetInfo.isDead == false && canAttackTarget) {
                mainPerson.personState.stateMachineMoving = true;
                mainPerson.personState.stateMachineTargeting = true;
            }
            else {
                mainPerson.SetTarget(null);
            }
        }

        if(mainPerson.destination) {
            Vector3 destinationDirection = (mainPerson.destination.position - mainPerson.transform.position).normalized;
            destinationDirection.y = 0;
            if(destinationDirection.magnitude != Vector3.zero.magnitude) {
                Quaternion lookRotation = Quaternion.LookRotation(destinationDirection);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);
            }

            destinationDistance =
            animator.GetBehaviour<MoveState>().reposition != Vector3.zero ?
            Vector3.Distance(animator.GetBehaviour<MoveState>().reposition, mainPerson.transform.position) :
            Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance > reachDistance) {
                mainPerson.personState.stateMachineMoving = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}