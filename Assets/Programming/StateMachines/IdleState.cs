using UnityEngine;

public class IdleState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;
    public bool reachDestination = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        targetInfo = mainPerson.target.GetComponent<Info>();
  
        mainPerson.personAgent.isStopped = true;

        destinationDistance = Vector3.Distance(mainPerson.personDestination.position, mainPerson.transform.position);
        if(destinationDistance <= (mainPerson.personAgent.stoppingDistance + 1f) && mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
            reachDestination = true;
        } else {
            reachDestination = false;
        }

        if(mainPerson.personDestination && reachDestination == false) {
            mainPerson.ChangeState(StateMachine.Move);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
        if(targetDistance <= 250f && targetInfo.isDead == false) {
            reachDestination = false;
            mainPerson.ChangeState(StateMachine.Follow);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}