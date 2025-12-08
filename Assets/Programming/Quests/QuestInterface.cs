using UnityEngine;

public interface QuestInterface {
    QuestSerializable questDetails {get; set;}
    bool isActive {get; set;}
    bool isComplete {get; set;}
    void StartQuest();
    void CheckQuest();
    void RestartQuest();
}