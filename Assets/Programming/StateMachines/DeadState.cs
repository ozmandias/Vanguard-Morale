using UnityEngine;

public class DeadState : StateMachineBehaviour {
    Person mainPerson;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        if(mainPerson.personInfo.isDead == true) {
            if(mainPerson.target) {
                CombatManager targetCombat = mainPerson.target.GetComponent<CombatManager>();
                if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                    targetCombat.circlingList.Remove(mainPerson.personAgent);
                }
                mainPerson.SetTarget(null);
            }
            mainPerson.attackingTarget = false;
            mainPerson.personCombat.circlingList.Clear();
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