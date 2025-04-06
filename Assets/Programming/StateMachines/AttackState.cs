using UnityEngine;

public class AttackState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float targetDistance;
    float stateTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        targetInfo = mainPerson.target.GetComponent<Info>();

        mainPerson.personAgent.isStopped = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
        targetDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);
        
        targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);

        if(stateTime == 0) {
            mainPerson.isAttacking = true;
            mainPerson.attackCollider.enabled = true;
        }

        if(mainPerson.isAttacking == true) {
            stateTime = stateInfo.normalizedTime % 1;
            if(stateTime > 0.9f) {
                mainPerson.isAttacking = false;
                mainPerson.attackCollider.enabled = false;
                stateTime = 0;

                if(targetDistance > 10f && targetInfo.isDead == false) {
                    mainPerson.ChangeState(StateMachine.Follow);
                } else if(targetDistance > 250f || targetInfo.isDead == true) {
                    mainPerson.ChangeState(StateMachine.Move);
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson.isAttacking = false;
        mainPerson.attackCollider.enabled = false;
        stateTime = 0;
    }
}