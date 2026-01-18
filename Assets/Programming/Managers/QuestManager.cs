using UnityEngine;

public class QuestManager : MonoBehaviour { // for AI
    public Person mainPerson;
    public Quest mainQuest;
    public BehaviourSerializable questBehaviourSerializable;

    void Start() {
        mainPerson = GetComponent<Person>();
        /*switch(mainQuest.questDetails.questInfo[mainQuest.questInfoId].questType) {
            case QuestType.Kill:
                // set up target and destination
                break;
            default:
                break;
        }*/
    }

    void Update() {
        if(mainQuest) {
            switch(mainQuest.questDetails.questInfo[mainQuest.questInfoId].questType) {
                case QuestType.Kill:
                    ManageKillQuest();
                    break;
                default:
                    break;
            }
        }
    }

    // for NPCs
    void ManageKillQuest() {
        if(mainPerson.GetInfo().isDead) {
            (mainQuest as KillQuest).toKillList.Remove(gameObject);
        }
    }

    void ManageCollectQuest() {

    }

    void ManageTalkQuest() {

    }

    void ManageTravelQuest() {

    }

    void ManageProtectQuest() {

    }

    void ManageDestroyQuest() {

    }
}