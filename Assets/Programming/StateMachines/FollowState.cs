using UnityEngine;

public class FollowState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float targetDistance;
    public bool nearTarget = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        targetInfo = mainPerson.personTarget.GetComponent<Info>();

        mainPerson.personAgent.isStopped = false;

        targetDistance = Vector3.Distance(mainPerson.personTarget.transform.position, mainPerson.transform.position);
        if(targetDistance > 10f) {
            nearTarget = false;
        } else {
            nearTarget = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        targetDistance = Vector3.Distance(mainPerson.personTarget.transform.position, mainPerson.transform.position);

        if(targetDistance <= 100f && targetDistance > 10f && targetInfo.isDead == false && nearTarget == false) {
            mainPerson.personAgent.destination = mainPerson.personTarget.transform.position;
        } else if(targetDistance <= 10f && targetInfo.isDead == false) {
            nearTarget = true;
            mainPerson.ChangeState(StateMachine.Attack);
        }
        
        if(targetDistance > 100f || targetInfo.isDead == true) {
            mainPerson.ChangeState(StateMachine.Move);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson.personAgent.ResetPath();
    }
}