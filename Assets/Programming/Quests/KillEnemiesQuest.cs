using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEnemiesQuest : Quest {
    public List<GameObject> enemiesToKillList;

    public override void StartQuest() {
        isComplete = false;
    }

    public override void CheckQuest() {
        if(enemiesToKillList.Count == 0) {
            isComplete = true;
        }
    }

    public override void RestartQuest() {
        StartQuest();
    }
}