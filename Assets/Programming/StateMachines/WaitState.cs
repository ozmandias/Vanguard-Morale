using UnityEngine;

public class WaitState : StateMachineBehaviour {
    Person mainPerson;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Wait in line for current target or move on to other target
        if(mainPerson.target) {
            // mainPerson.SetTarget(null);
            Info targetInfo = mainPerson.target.GetComponent<Info>();
            CombatManager targetCombat = mainPerson.target.GetComponent<CombatManager>();

            if(targetCombat.IsCirclingListFull() == false && targetInfo.isDead == false) {
                mainPerson.ChangeState(StateMachine.Follow);
            } else {
                mainPerson.SetTarget(null);
                mainPerson.FindTarget();
            }
        } else {
            mainPerson.ChangeState(StateMachine.Move);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}