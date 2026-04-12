using UnityEngine;

public class WorldManager : MonoBehaviour {
    public WorldScriptableObject worldScriptableObject;

    public static WorldManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {

    }

    void Update() {

    }
}