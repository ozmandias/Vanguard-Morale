using UnityEngine;

public class DeadState : StateMachineBehaviour {
    Person mainPerson;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        if(mainPerson.personInfo.isDead == true) {
            mainPerson.attackingTarget = false;
            mainPerson.personAgent.enabled = false;
            mainPerson.personInfo.stateMachineDead = true;
            mainPerson.personInfo.personRagdollManager.EnableRagdoll();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}