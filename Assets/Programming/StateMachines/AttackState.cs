using UnityEngine;

public class AttackState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float targetDistance;
    float stateTime = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        mainPerson.personAgent.isStopped = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            CombatManager targetCombat = mainPerson.target.GetComponent<CombatManager>();
            
            Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
            targetDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);

            stateTime = stateInfo.normalizedTime % 1;

            if(stateTime > 0.1f) {
                mainPerson.isAttacking = true;
            }

            if(mainPerson.isAttacking == true) {
                mainPerson.attackCollider.enabled = true;
                if(stateTime > 0.9f) {
                    mainPerson.isAttacking = false;
                    mainPerson.attackCollider.enabled = false;
                    stateTime = 0;

                    targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                    if(targetDistance > 10f && targetInfo.isDead == false) {
                        mainPerson.nearTarget = false;
                    } else if(targetDistance > 250f || targetInfo.isDead == true) {
                        if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                            targetCombat.circlingList.Remove(mainPerson.personAgent);
                        }
                        mainPerson.SetTarget(null);
                        mainPerson.attackingTarget = false;
                    }
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson.isAttacking = false;
        mainPerson.attackingTarget = false;
        mainPerson.nearTarget = false;
        mainPerson.attackCollider.enabled = false;
        stateTime = 0;
    }
}