using UnityEngine;

[System.Serializable] public class QuestSerializable {
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
    public ToKillSpawnSerializable []toKillSpawnSerializables;
    public RewardSpawnSerializable []rewardSpawnSerializables;
}