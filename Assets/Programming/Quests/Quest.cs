using UnityEngine;
using UnityEngine.UI;

public abstract class Quest : MonoBehaviour, QuestInterface {
    void Start() {
        StartQuest();
    }

    void Update() {
        if(isComplete == false) {
            CheckQuest();
        }
    }

    public Text questInfoText {get; set;}
    public bool isComplete {get; set;}
    public abstract void StartQuest();
    public abstract void CheckQuest();
    public abstract void RestartQuest();
}