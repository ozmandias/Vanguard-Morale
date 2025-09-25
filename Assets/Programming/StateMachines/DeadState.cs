using UnityEngine;

public class DeadState : StateMachineBehaviour {
    Person mainPerson;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;

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
            mainPerson.personAgent.enabled = false;
            mainPerson.GetInfo().stateMachineDead = true;
            mainPerson.GetInfo().personRagdollManager.EnableRagdoll();
            if (mainPerson.weapon)
            {
                mainPerson.weapon.transform.SetParent(null);
                mainPerson.weapon.AddComponent<Rigidbody>();
                mainPerson.weapon.AddComponent<BoxCollider>();
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}