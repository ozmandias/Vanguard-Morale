using UnityEngine;

public class DeadState : StateMachineBehaviour {
    Person mainPerson;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;
        mainPerson.personAgent.isStopped = true;

        if (mainPerson.GetInfo().isDead == true)
        {
            if (mainPerson.target)
            {
                CombatManager targetCombat = mainPerson.target.GetComponent<CombatManager>();
                if(mainPerson.target.CompareTag("Player")) {
                    if(targetCombat.CombatingListContains(mainPerson.personAgent)) {
                        targetCombat.combatingList.Remove(mainPerson.personAgent);
                    }
                } else if(mainPerson.target.CompareTag("Person")) {
                    if (targetCombat.CirclingListContains(mainPerson.personAgent))
                    {
                        targetCombat.circlingList.Remove(mainPerson.personAgent);
                    }
                }
            }
            mainPerson.personState.stateMachineAttacking = false;
            mainPerson.personState.stateMachineTargeting = false;
            mainPerson.personCombat.circlingList.Clear();
            mainPerson.personCombat.enemyInCombat = false;
            mainPerson.personCombat.counterAlert = false;
            mainPerson.SetTarget(null);
            mainPerson.GetInfo().MakeLife("dead");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}