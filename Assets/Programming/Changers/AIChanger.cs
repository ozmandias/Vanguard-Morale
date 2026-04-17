using UnityEngine;

public class AIChanger : MonoBehaviour {
    // make AI Types universal
    // change AI Types to State Machine, Combat (or) Quest
    Person mainPerson;
    public AIType aiType = AIType.StateMachine;
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
                switch(aiType) {
                    case AIType.StateMachine:
                        if(mainPerson.personQuest.mainQuest) {
                            if(mainPerson.personQuest.mainQuest.isActive) {
                                ChangeAI("questAI");
                            }
                        }

                        if(
                            (mainPerson.GetInfo().personType == PersonType.Enemy && (GameManager.instance.playerGameObject != null && mainPerson.target == GameManager.instance.playerGameObject))
                            ||
                            mainPerson.personCombat.enemyInCombat
                        )
                        {
                            Debug.Log("gameObject: " + gameObject.name + " - stateMachine to combatAI - playerTarget: " + GameManager.instance.playerGameObject);
                            ChangeAI("combatAI");
                        }
                        break;
                    case AIType.QuestAI:
                        if(mainPerson.personQuest.mainQuest.isComplete) {
                            ChangeAI("stateMachine");
                        }

                        if(
                            (mainPerson.GetInfo().personType == PersonType.Enemy && (GameManager.instance.playerGameObject != null && mainPerson.target == GameManager.instance.playerGameObject))
                            ||
                            mainPerson.personCombat.enemyInCombat
                        )
                        {
                            Debug.Log("gameObject: " + gameObject.name + " - questAI to combatAI");
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
            }
        }
    }

    public void ChangeAI(string aiStatus) {
        if(aiStatus == "stateMachine") {
            aiType = AIType.StateMachine;
        } else if(aiStatus == "combatAI") {
            aiType = AIType.CombatAI;
        } else if(aiStatus == "questAI") {
            aiType = AIType.QuestAI;
        }
    }
}