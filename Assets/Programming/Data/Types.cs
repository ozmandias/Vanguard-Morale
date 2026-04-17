using UnityEngine;
using UnityEngine.UI;

public class KeyValue<TKey, TValue> {
    public TKey key;
    public TValue value;
}

// rename prefab to asset
public class Asset {
    public GameObject assetObject;
    public string assetName;
    public Sprite assetSprite;
}

public class Spawn : Asset {
    public int spawnCount;
    // public Transform []spawnTransforms;
    public Vector3 []spawnPositions;
}

public class Spawn<T> : Asset {
    public int spawnCount;
    // public Transform []spawnTransforms;
    public Vector3 []spawnPositions;
    public T spawnType;
}