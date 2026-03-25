using UnityEngine;

public class MoveState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    float destinationDistance;
    float targetDistance;

    public Vector3 reposition;
    public float repositionDistance = 100f;
    public float reachDistance = 0;
    public float followDistance = 250f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.attackNumberUpdate = false;
        mainPerson.personAgent.isStopped = false;

        reachDistance = mainPerson.personAgent.stoppingDistance;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canAttackTarget = true;

            // targetInfo = mainPerson.target.CompareTag("Player") ? GameManager.instance.currentPlayer == PlayerCharacter.Vanguard ? (Info) mainPerson.target.GetComponent<Vanguard>().GetInfo() : (Info) mainPerson.target.GetComponent<Player>().GetInfo() : (Info) mainPerson.target.GetComponent<Person>().GetInfo();
            targetInfo = mainPerson.target.CompareTag("Player") ? PlayerManager.instance.playerGameObject.GetComponent<Vanguard>() != null ? (Info) mainPerson.target.GetComponent<Vanguard>().GetInfo() : (Info) mainPerson.target.GetComponent<Player>().GetInfo() : (Info) mainPerson.target.GetComponent<Person>().GetInfo();
            if (targetInfo is PersonInfo) {
                if ((targetInfo as PersonInfo).person.personAI.aiType == AIType.CombatAI) {
                    canAttackTarget = false;
                }
            }
            
            targetDistance = Vector3.Distance(mainPerson.target.transform.position, mainPerson.transform.position);
            if(targetDistance <= followDistance && targetInfo.isDead == false && canAttackTarget) {
                mainPerson.personState.stateMachineTargeting = true;
            }
            else {
                mainPerson.SetTarget(null);
            }
        }

        if(mainPerson.destination) {
            destinationDistance = Vector3.Distance(mainPerson.destination.position, mainPerson.transform.position);
            if(reposition != Vector3.zero) {
                // check with reposition
                destinationDistance = Vector3.Distance(reposition, mainPerson.transform.position);
            }
            if(destinationDistance > repositionDistance) {
                mainPerson.personAgent.destination = mainPerson.destination.position;
            }
            else if(destinationDistance <= repositionDistance && destinationDistance > reachDistance) {
                // return a reposition, calculate only one time
                reposition = AIManager.instance.AgentRepositionAtDestination(mainPerson.personAgent, mainPerson.destination, mainPerson.GetInfo().personType);
            }
            else if(destinationDistance <= reachDistance) {
                mainPerson.personAgent.velocity = Vector3.zero;
                mainPerson.personState.stateMachineMoving = false;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*if(mainPerson.GetInfo().isDead == false) {
            mainPerson.personAgent.ResetPath();
        }*/
    }
}