using UnityEngine;

public class IdleState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public float reachDistance = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        reachDistance = AIManager.instance.DistanceAroundDestination /*mainPerson.personAgent.stoppingDistance*/ + 1f;
  
        mainPerson.personAgent.isStopped = true;
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

        if(mainPerson.destination) {
            Vector3 destinationDirection = (mainPerson.destination.position - mainPerson.transform.position).normalized;
            destinationDirection.y = 0;
            if(destinationDirection.magnitude != Vector3.zero.magnitude) {
                Quaternion lookRotation = Quaternion.LookRotation(destinationDirection);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);
            }

            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance > reachDistance) {
                mainPerson.reachDestination = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}