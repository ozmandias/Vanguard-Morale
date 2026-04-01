using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarManager : MonoBehaviour {
    // two war teams
    WarTeam homeWarTeam;
    WarTeam awayWarTeam;

    void Start() {
        DetermineWarTeams();
    }

    void Update() {
        CheckWar();
    }

    void DetermineWarTeams() {
        // get 2 factions at war based on reputation on each other
        // 1 faction is current kingdom and another faction is the one with bad reputation to current kingdom
        CreateWarTeams();
        
    }

    void CreateWarTeams() {
        // create 2 war teams based on 2 faction pairs
        CreateSoldiers();
    }

    void CreateSoldiers() {
        StartCoroutine(CreateSoldiersCoroutine());
    }

    void CheckWar() {
        // if isDestroyed from any WarTeam's Info class is true, end war and declare winner and loser
    }

    IEnumerator CreateSoldiersCoroutine() {
        yield return null;
        // create soldiers at army base
        // register soldiers to factionSoldiers list
    }
}