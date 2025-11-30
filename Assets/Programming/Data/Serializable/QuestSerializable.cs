using UnityEngine;

[System.Serializable] public class QuestSerializable {
    public string questTitle;
    public QuestInfo []questInfo;
}

[System.Serializable] public class QuestInfo {
    public string questDescription;
    public QuestType questType;
    public RewardType rewardType;
    public GameObject questObject;
}