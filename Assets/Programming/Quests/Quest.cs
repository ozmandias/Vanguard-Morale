using UnityEngine;

public abstract class Quest : MonoBehaviour, QuestInterface {
    public virtual void Start() {
        StartQuest();
    }

    public virtual void Update() {
        if(isActive == true && isComplete == false) {
            CheckQuest();
        }
    }

    public QuestModel questDetails {get; set;}
    public int questInfoId {get; set;}
    public bool isActive {get; set;}
    public bool isComplete {get; set;}
    public abstract void StartQuest();
    public abstract void CheckQuest();
    public abstract void RestartQuest();
}