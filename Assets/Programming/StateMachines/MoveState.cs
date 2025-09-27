using UnityEngine;

public class MoveState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public float repositionDistance = 100f;
    public float reachDistance = 0;
    public float followDistance = 250f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;
        mainPerson.personAgent.isStopped = false;

        reachDistance = AIManager.instance.DistanceAroundDestination /*mainPerson.personAgent.stoppingDistance*/ + 1f;
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
                mainPerson.attackingTarget = true;
            } else if(targetInfo.isDead == true) {
                mainPerson.SetTarget(null);
            }
        }

        if(mainPerson.destination) {
            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(destinationDistance > reachDistance && destinationDistance > repositionDistance) {
                mainPerson.personAgent.destination = mainPerson.destination.position;
            } else if(destinationDistance <= repositionDistance && destinationDistance > reachDistance) {
                AIManager.instance.AgentRepositionAtDestination(mainPerson.GetInfo().personType, mainPerson.personAgent, mainPerson.destination);
            } else if(destinationDistance <= reachDistance) {
                mainPerson.personAgent.velocity = Vector3.zero;
                if(mainPerson.personAgent.velocity.magnitude <= Vector3.zero.magnitude) {
                    mainPerson.reachDestination = true;
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.GetInfo().isDead == false) {
            mainPerson.personAgent.ResetPath();
        }
    }
}