using UnityEngine;

public class AIChanger : MonoBehaviour {
    // make AI Types universal
    // change AI Types to State Machine, Combat (or) Quest
    Person mainPerson;
    public delegate void ChangeAIDelegate(string aiStatus);
    public ChangeAIDelegate OnChangeAIDelegate;

    void Start() {
        mainPerson = GetComponent<Person>();

        OnChangeAIDelegate += MakeAI;
    }

    void Update() {
    }

    void ChangeAI() {
        if (mainPerson.GetInfo().personType == PersonType.Enemy && mainPerson.target == GameManager.instance.playerGameObject)
        {
            MakeAI("combatAI");
        }

        if(mainPerson.GetInfo().isDead) {
            MakeAI("stateMachine");
        }
    }

    public void MakeAI(string aiStatus) {
        if(aiStatus == "stateMachine") {
            mainPerson.GetInfo().aiType = AIType.StateMachine;
        } else if(aiStatus == "combatAI") {
            mainPerson.GetInfo().aiType = AIType.CombatAI;
        } else if(aiStatus == "questAI") {
            mainPerson.GetInfo().aiType = AIType.QuestAI;
        }
    }
}