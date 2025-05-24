using UnityEngine;

public class AttackState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    CombatManager targetCombat;
    float targetDistance;
    [SerializeField] float stateTime = 0;

    public float followDistance = 250f;
    public float nearDistance = 10f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

        mainPerson.personAgent.isStopped = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            targetInfo = mainPerson.target.GetComponent<Info>();
            targetCombat = mainPerson.target.GetComponent<CombatManager>();

            Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
            targetDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);

            if(animator.GetBool("Attacking") == true) {
                mainPerson.isAttacking = true;
                mainPerson.attackCollider.enabled = true;
            } else {
                mainPerson.isAttacking = false;
                mainPerson.attackCollider.enabled = false;
                stateTime = 0;
            }

            if(mainPerson.isAttacking == true) {
                stateTime += Time.deltaTime /*stateInfo.normalizedTime % 1*/;
                targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                if(targetDistance > nearDistance && targetInfo.isDead == false) {
                    mainPerson.nearTarget = false;
                } else if(targetDistance > followDistance || targetInfo.isDead == true) {
                    if(targetCombat.CirclingListContains(mainPerson.personAgent)) {
                        targetCombat.circlingList.Remove(mainPerson.personAgent);
                    }
                    mainPerson.SetTarget(null);
                    mainPerson.attackingTarget = false;
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("Attacking", false);
        mainPerson.isAttacking = false;
        mainPerson.attackCollider.enabled = false;
        stateTime = 0;
    }
}