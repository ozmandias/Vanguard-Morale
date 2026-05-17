using UnityEngine;

// use structs when I cannot give any names or other names for specific class creation are taken
// structs can be used in Manager classes to store run-time data
// structs are light-weight and faster in Memory; cannot be inherited

[System.Serializable] public struct EnemyStruct
{
    public Enemy enemy;
    public bool available;
}

[System.Serializable] public struct QuestStruct {
    public QuestType questType;
    public GameObject questObject;
}

[System.Serializable] public struct SceneStruct {
    public GameObject sceneUIObject;
}

[System.Serializable] public struct SoundStruct {
    
}