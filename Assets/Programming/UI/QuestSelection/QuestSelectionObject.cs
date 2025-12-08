using UnityEngine;

public class QuestSelectionObject : MonoBehaviour { // for Quest's UI
    public Quest quest;
    public bool active = false;

    void Start() {
        quest = GetComponent<Quest>();
    }
}