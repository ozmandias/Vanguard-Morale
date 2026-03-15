using UnityEngine;

public interface QuestInterface {
    QuestModel questDetails {get; set;}
    int questInfoId {get; set;}
    bool isActive {get; set;}
    bool isComplete {get; set;}
    void StartQuest();
    void CheckQuest();
    void RestartQuest();
}