using UnityEngine;

public class FollowState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float targetDistance;
    public bool nearTarget = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        targetInfo = mainPerson.target.GetComponent<Info>();

        mainPerson.personAgent.isStopped = false;
        mainPerson.attackingTarget = true;

        targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
        if(targetDistance > 10f) {
            nearTarget = false;
            mainPerson.personAgent.destination = mainPerson.target.transform.position;
        } else {
            nearTarget = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);

        if(targetDistance <= 250f && targetDistance > 10f && targetInfo.isDead == false && nearTarget == false) {
            mainPerson.personAgent.destination = mainPerson.target.transform.position;
        } else if(targetDistance <= 10f && targetInfo.isDead == false) {
            nearTarget = true;
            mainPerson.ChangeState(StateMachine.Attack);
        }
        
        if(targetDistance > 250f || targetInfo.isDead == true) {
            mainPerson.ChangeState(StateMachine.Move);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.personInfo.isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }
}