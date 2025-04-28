using UnityEngine;

public class MoveState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        
        mainPerson.personAgent.isStopped = false;

        if(mainPerson.destination) {
            mainPerson.personAgent.destination = mainPerson.destination.position;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance < destinationDistance && targetDistance <= 250f && targetInfo.isDead == false) {
                mainPerson.attackingTarget = true;
            }
        }

        if(mainPerson.destination) {
            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance <= 100f && destinationDistance > (AIManager.instance.DistanceAroundDestination + 1f)) {
                // AIManager.instance.AgentCircleTarget(mainPerson.personInfo.personType, mainPerson.personAgent, mainPerson.destination, CircleType.Semicircle);
                AIManager.instance.AgentRepositionAtDestination(mainPerson.personInfo.personType, mainPerson.personAgent, mainPerson.destination);
            }else if(destinationDistance <= (AIManager.instance.DistanceAroundDestination /*mainPerson.personAgent.stoppingDistance*/ + 1f) && mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
                mainPerson.reachDestination = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.personInfo.isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }

}