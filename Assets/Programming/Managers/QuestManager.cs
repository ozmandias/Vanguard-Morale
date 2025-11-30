using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {
    [SerializeField] QuestScriptableObject questScriptableObject;
    [SerializeField] QuestSerializable currentQuest;

    public static QuestManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        questScriptableObject = GlobalData.currentKingdomQuestScriptableObject;
    }

    void Update() {

    }

    public void LoadAllQuests() {

    }

    public void AcceptQuest() {
        
    }
}