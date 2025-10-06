using UnityEngine;

public class CombatState : StateMachineBehaviour {
    Person mainPerson;
    Info targetInfo;
    CombatManager targetCombat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        mainPerson = animator.gameObject.GetComponent<Person>();

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

            if(canCombatTarget == false) {
                //remove from CombatAI List
                AIManager.instance.RemoveCombatEnemy(mainPerson as Enemy);
                mainPerson.SetTarget(null);
                mainPerson.GetInfo().ChangeToStateMachineAI();
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }
}