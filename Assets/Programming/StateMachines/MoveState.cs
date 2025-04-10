using UnityEngine;

public class MoveState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        
        mainPerson.personAgent.isStopped = false;
        mainPerson.attackingTarget = false;

        if(mainPerson.personDestination) {
            mainPerson.personAgent.destination = mainPerson.personDestination.position;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance < destinationDistance && targetDistance <= 250f && targetInfo.isDead == false) {
                mainPerson.ChangeState(StateMachine.Follow);
            }
        }

        if(mainPerson.personDestination) {
            destinationDistance = Vector3.Distance(mainPerson.personDestination.position, mainPerson.transform.position);
            if(destinationDistance <= (mainPerson.personAgent.stoppingDistance + 1f) && mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
                mainPerson.ChangeState(StateMachine.Idle);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.personInfo.isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }

}