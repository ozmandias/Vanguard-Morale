using UnityEngine;
using UnityEngine.UI;

public interface QuestInterface {
    Text questInfoText {get; set;}
    bool isComplete {get; set;}
    void StartQuest();
    void CheckQuest();
    void RestartQuest();
}