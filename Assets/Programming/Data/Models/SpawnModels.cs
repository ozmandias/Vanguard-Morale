using UnityEngine;
using UnityEngine.UI;

// toKillSpawn
[System.Serializable] public class ToKillSpawnModel : Spawn {
    public PersonType personType;
    public BehaviourModel behaviourModel;
}

// toCollectSpawn
[System.Serializable] public class ToCollectSpawnModel : Spawn {
    public RewardType rewardType;
}

// toTalkSpawn
[System.Serializable] public class ToTalkSpawnModel : Spawn {
    public PersonType personType;
}

// toTravelSpawn
[System.Serializable] public class ToTravelSpawnModel : Spawn {

}

// toProtectSpawn
[System.Serializable] public class ToProtectSpawnModel : Spawn {
    public PersonType personType;
}

// toDestroySpawn
[System.Serializable] public class ToDestroySpawnModel : Spawn {

}

[System.Serializable] public class RewardSpawnModel : Spawn {
    public RewardType rewardType;
}