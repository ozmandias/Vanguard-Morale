using UnityEngine;

[System.Serializable] public class QuestInfo {
    public string questDescription;
    public QuestType questType;
    public RewardType rewardType;
    // make a data structure for different questObjects and their counts
    /*public GameObject []questObjects;
    public int singleQuestObjectCount;*/
    public KillQuestSpawnModel []killQuestSpawnModels;
    public CollectQuestSpawnModel []collectQuestSpawnModels;
    public TalkQuestSpawnModel []talkQuestSpawnModels;
    public TravelQuestSpawnModel []travelQuestSpawnModels;
    public ProtectQuestSpawnModel []protectQuestSpawnModels;
    public DestroyQuestSpawnModel []destroyQuestSpawnModels;
    public RewardSpawnModel []rewardSpawnModels;
}