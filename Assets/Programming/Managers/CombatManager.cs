using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatManager : MonoBehaviour {
    public List<NavMeshAgent> circlingList;
    public int maxCirclingEnemies = 3;
    public bool runAICircling = false;

    float counterTimer = 0;
    public void CounterAttack(float attackTimer) {
        counterTimer += Time.deltaTime;
        if(counterTimer < attackTimer / 2) {
        }
    }

    public bool IsCirclingListFull() {
        bool fullStatus = circlingList.Count == maxCirclingEnemies ? true : false;
        runAICircling = fullStatus == true ? true : false;
        return fullStatus;
    }

    public bool CirclingListContains(NavMeshAgent circlingAgent) {
        bool containStatus = circlingList.Contains(circlingAgent);
        return containStatus;
    }
}