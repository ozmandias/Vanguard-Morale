using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class SpawnModel {
    public GameObject spawnObject;
    public int spawnCount;
    public Vector3 []spawnPositions;
}

// KillQuestSpawnModel
[System.Serializable] public class KillQuestSpawnModel : SpawnModel {
    public PersonType personType;
    public BehaviourModel behaviourModel;
}

// CollectQuestSpawnModel
[System.Serializable] public class CollectQuestSpawnModel : SpawnModel {
    public RewardType rewardType;
}

// TalkQuestSpawnModel
[System.Serializable] public class TalkQuestSpawnModel : SpawnModel {
    public PersonType personType;
}

// TravelQuestSpawnModel
[System.Serializable] public class TravelQuestSpawnModel : SpawnModel {

}

// ProtectQuestSpawnModel
[System.Serializable] public class ProtectQuestSpawnModel : SpawnModel {
    public PersonType personType;
}

// DestroyQuestSpawnModel
[System.Serializable] public class DestroyQuestSpawnModel : SpawnModel {

}

[System.Serializable] public class RewardSpawnModel : SpawnModel {
    public RewardType rewardType;
}