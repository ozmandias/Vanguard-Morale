using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectQuest : Quest {
    public List<GameObject> toProtectList = new List<GameObject>();
    public Transform protectTransform;
    public float completeDistance = 3f;

    public override void Update() {
        base.Update();
        // move each personToProtect to protectTransform
    }

    public override void StartQuest() {
        isComplete = false;
    }

    public override void CheckQuest() {
        foreach(GameObject personToProtect in toProtectList) {
            if(Vector3.Distance(protectTransform.position, personToProtect.transform.position) <= completeDistance) {
                isComplete = true;
                isActive = false;
            }
        }
    }

    public override void RestartQuest() {
        
    }
}