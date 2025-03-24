using UnityEngine;

public class MoveState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        targetInfo = mainPerson.personTarget.GetComponent<Info>();

        mainPerson.personAgent.isStopped = false;

        mainPerson.personAgent.destination = mainPerson.personDestination.position;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        destinationDistance = Vector3.Distance(mainPerson.personDestination.position, mainPerson.transform.position);
        targetDistance = Vector3.Distance(mainPerson.personTarget.transform.position, mainPerson.transform.position);
        
        if(targetDistance < destinationDistance && targetDistance <= 100f && targetInfo.isDead == false) {
            mainPerson.ChangeState(StateMachine.Follow);
        }
        
        if(destinationDistance <= (mainPerson.personAgent.stoppingDistance + 1f) && mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
            mainPerson.ChangeState(StateMachine.Idle);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson.personAgent.ResetPath();
    }

}