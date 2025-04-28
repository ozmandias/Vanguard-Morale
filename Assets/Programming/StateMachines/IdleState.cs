using UnityEngine;

public class IdleState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
  
        mainPerson.personAgent.isStopped = true;

        if(mainPerson.destination) {
            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance <= (AIManager.instance.DistanceAroundDestination /*mainPerson.personAgent.stoppingDistance*/ + 1f) && mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
                mainPerson.reachDestination = true;
            } else {
                mainPerson.reachDestination = false;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance <= 250f && targetInfo.isDead == false) {
                mainPerson.reachDestination = false;
                mainPerson.attackingTarget = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}