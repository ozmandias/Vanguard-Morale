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
            
            Vector3 targetDirection = (mainPerson.target.transform.position - animator.transform.position).normalized;
            targetDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);

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

                    targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
                    if(targetDistance > 10f && targetInfo.isDead == false) {
                        mainPerson.ChangeState(StateMachine.Follow);
                    } else if(targetDistance > 250f || targetInfo.isDead == true) {
                        mainPerson.SetTarget(null);
                        if(mainPerson.personDestination) {
                            mainPerson.ChangeState(StateMachine.Move);
                        } else {
                            mainPerson.ChangeState(StateMachine.Idle);
                        }
                    }
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