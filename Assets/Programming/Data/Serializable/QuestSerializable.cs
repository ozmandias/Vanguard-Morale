using UnityEngine;

[System.Serializable] public class QuestSerializable {
    public string questTitle;
    public string questDescription;
    public QuestType questType;
    public RewardType rewardType;
    public GameObject []questObjects;
}