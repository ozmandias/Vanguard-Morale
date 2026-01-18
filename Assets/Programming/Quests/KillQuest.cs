using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillQuest : Quest { // for UI
    public List<GameObject> toKillList = new List<GameObject>();

    public override void StartQuest() {
        isActive = true;
        isComplete = false;
    }

    public override void CheckQuest() {
        if(toKillList.Count == 0) {
            isComplete = true;
            isActive = false;
        }
    }

    public override void RestartQuest() {
        StartQuest();
    }
}