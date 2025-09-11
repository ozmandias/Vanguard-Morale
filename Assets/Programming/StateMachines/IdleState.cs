using UnityEngine;

public class IdleState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public float reachDistance = 0;
    public float followDistance = 250f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        reachDistance = AIManager.instance.DistanceAroundDestination /*mainPerson.personAgent.stoppingDistance*/ + 1f;
  
        mainPerson.personAgent.isStopped = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? (Info) mainPerson.target.GetComponent<MasterKnight>().GetInfo() : (Info) mainPerson.target.GetComponent<Player>().GetInfo() : (Info) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance <= followDistance && targetInfo.isDead == false && canAttackTarget) {
                mainPerson.reachDestination = false;
                mainPerson.attackingTarget = true;
            }
        }

        if(mainPerson.destination) {
            Vector3 destinationDirection = (mainPerson.destination.position - mainPerson.transform.position).normalized;
            destinationDirection.y = 0;
            if(destinationDirection.magnitude != Vector3.zero.magnitude) {
                Quaternion lookRotation = Quaternion.LookRotation(destinationDirection);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, mainPerson.speed * Time.deltaTime);
            }

            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance > reachDistance) {
                mainPerson.reachDestination = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}