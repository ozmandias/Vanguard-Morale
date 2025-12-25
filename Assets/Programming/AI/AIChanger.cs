using UnityEngine;

public class AIChanger : MonoBehaviour {
    // make AI Types universal
    // change AI Types to State Machine, Combat (or) Quest
    Person mainPerson;
    public bool aiChangerRunning = false;
    
    public delegate void ChangeAIDelegate(string aiStatus);
    public ChangeAIDelegate OnChangeAIDelegate;

    void Start() {
        mainPerson = GetComponent<Person>();
        aiChangerRunning = true;

        OnChangeAIDelegate += ChangeAI;
    }

    void Update() {
        CheckAI();
    }

    void CheckAI() {
        if(aiChangerRunning) {
            if(mainPerson.GetInfo().isDead == false) {
                switch(mainPerson.GetInfo().aiType) {
                    case AIType.StateMachine:
                        if(mainPerson.personQuest.mainQuest) {
                            if(mainPerson.personQuest.mainQuest.isActive) {
                                ChangeAI("questAI");
                            }
                        }

                        if(
                            (mainPerson.target == GameManager.instance.playerGameObject || mainPerson.personCombat.enemyInCombat)
                            &&
                            GameHelpers.GetCharacterCombat(GameManager.instance.playerGameObject).IsCombatingListFull() == false
                        )
                        {
                            ChangeAI("combatAI");
                        }
                        break;
                    case AIType.QuestAI:
                        if(mainPerson.personQuest.mainQuest.isComplete) {
                            ChangeAI("stateMachine");
                        }

                        if(
                            ((mainPerson.GetInfo().personType == PersonType.Enemy && mainPerson.target == GameManager.instance.playerGameObject)
                            ||
                            (mainPerson.personCombat.enemyInCombat))
                            &&
                            (GameHelpers.GetCharacterCombat(GameManager.instance.playerGameObject).IsCombatingListFull() == false)
                        )
                        {
                            ChangeAI("combatAI");
                        }
                        break;
                    case AIType.CombatAI:
                        if(!mainPerson.personCombat.enemyInCombat) {
                            if(!mainPerson.personQuest.mainQuest) {
                                ChangeAI("stateMachine");
                            } else {
                                ChangeAI("questAI");
                            }
                        }
                        break;
                    case AIType.BossAI:
                        break;
                    default:
                        break;
                }
            } else {
                ChangeAI("stateMachine");
                aiChangerRunning = false;
            }
        }
    }

    public void ChangeAI(string aiStatus) {
        if(aiStatus == "stateMachine") {
            mainPerson.GetInfo().aiType = AIType.StateMachine;
        } else if(aiStatus == "combatAI") {
            mainPerson.GetInfo().aiType = AIType.CombatAI;
        } else if(aiStatus == "questAI") {
            mainPerson.GetInfo().aiType = AIType.QuestAI;
        }
    }
}