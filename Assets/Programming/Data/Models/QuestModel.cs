using UnityEngine;

[System.Serializable] public class QuestModel {
    public string questTitle;
    public QuestInfo []questInfo;
}

[System.Serializable] public class QuestInfo {
    public string questDescription;
    public QuestType questType;
    public RewardType rewardType;
    // make a data structure for different questObjects and their counts
    /*public GameObject []questObjects;
    public int singleQuestObjectCount;*/
    public ToKillSpawnModel []toKillSpawnModels;
    public ToCollectSpawnModel []toCollectSpawnModels;
    public ToTalkSpawnModel []toTalkSpawnModels;
    public ToTravelSpawnModel []toTravelSpawnModels;
    public ToProtectSpawnModel []toProtectSpawnModels;
    public ToDestroySpawnModel []toDestroySpawnModels;
    public RewardSpawnModel []rewardSpawnModels;
}