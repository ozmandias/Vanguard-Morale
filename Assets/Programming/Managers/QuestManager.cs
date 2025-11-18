using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {
    QuestScriptableObject questScriptableObject;
    QuestSerializable currentQuest;

    public static QuestManager instance;

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

    public void AcceptQuest() {
        
    }
}