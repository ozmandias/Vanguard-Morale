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
                if (targetCombat.CirclingListContains(mainPerson.personAgent))
                {
                    targetCombat.circlingList.Remove(mainPerson.personAgent);
                }
            }
            mainPerson.personCombat.circlingList.Clear();
            mainPerson.SetTarget(null);
            mainPerson.attackingTarget = false;
            mainPerson.GetInfo().MakeStateMachine("dead");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}