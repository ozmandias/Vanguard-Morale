using UnityEngine;

public class CombatState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    CombatManager targetCombat;
    float targetDistance;
    public float followDistance = 250f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();
        mainPerson.personAgent.isStopped = true;
        mainPerson.personCombat.OnEnemyStart.Invoke(mainPerson as Enemy);

        targetInfo = GameManager.instance.currentPlayer == PlayerCharacter.MasterKnight ? GameManager.instance.playerGameObject.GetComponent<MasterKnight>().GetInfo() as Info : GameManager.instance.playerGameObject.GetComponent<Player>().GetInfo() as Info;

        targetCombat = mainPerson.target.GetComponent<CombatManager>();
        if(targetCombat.CombatingListContains(mainPerson.personAgent) == false && targetCombat.IsCombatingListFull() == false) {
            targetCombat.combatingList.Add(mainPerson.personAgent);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mainPerson.target) {
            bool canCombatTarget = true;

            // check CombatingList and switch AIType if CombatingList is full
            if(targetCombat.CombatingListContains(mainPerson.personAgent) == false && targetCombat.IsCombatingListFull() == true) {
                canCombatTarget = false;
            }

            // if far from Player, change CombatAI to StateMachine
            if(targetCombat.CombatingListContains(mainPerson.personAgent)) {
                targetDistance = Vector3.Distance(targetCombat.transform.position, mainPerson.transform.position);
                if(targetDistance > followDistance) {
                    if(targetCombat.CombatingListContains(mainPerson.personAgent)) {
                        targetCombat.combatingList.Remove(mainPerson.personAgent);
                    }
                    canCombatTarget = false;
                }
            }

            if(canCombatTarget == false) {
                if(targetCombat.CombatingListContains(mainPerson.personAgent)) {
                    targetCombat.combatingList.Remove(mainPerson.personAgent);
                }
                //remove from CombatAI List
                AIManager.instance.RemoveCombatEnemy(mainPerson as Enemy);
                mainPerson.SetTarget(null);
                mainPerson.personCombat.OnEnemyStop.Invoke(mainPerson as Enemy);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }
}