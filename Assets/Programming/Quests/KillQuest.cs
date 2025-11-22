using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillQuest : Quest {
    public List<GameObject> toKillList;

    public override void StartQuest() {
        isComplete = false;
    }

    public override void CheckQuest() {
        if(toKillList.Count == 0) {
            isComplete = true;
        }
    }

    public override void RestartQuest() {
        StartQuest();
    }
}