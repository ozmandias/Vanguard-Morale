using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectQuest: Quest {
    public List<GameObject> toCollectList = new List<GameObject>();

    public override void StartQuest() {
        isComplete = false;
    }

    public override void CheckQuest() {
        if(isComplete == false) {
            if(toCollectList.Count == 0) {
                isComplete = true;
                isActive = false;
            }
        }
    }

    public override void RestartQuest() {
        StartQuest();
    }
}