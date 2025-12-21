using UnityEngine;
using UnityEngine.UI;

// toKillSpawn
[System.Serializable] public class ToKillSpawnSerializable : Spawn {
    public PersonType personType;
    public BehaviourSerializable behaviourSerializable;
}

// toCollectSpawn
[System.Serializable] public class ToCollectSpawnSerializable : Spawn {
    public RewardType rewardType;
}

// toTalkSpawn
[System.Serializable] public class ToTalkSpawnSerializble : Spawn {
    public PersonType personType;
}

// toTravelSpawn
[System.Serializable] public class ToTravelSpawnSerializable : Spawn {

}

// toProtectSpawn
[System.Serializable] public class ToProtectSpawnSerializable : Spawn {
    public PersonType personType;
}

// toDestroySpawn
[System.Serializable] public class ToDestroySpawnSerializable : Spawn {

}

[System.Serializable] public class RewardSpawnSerializable : Spawn {
    public RewardType rewardType;
}