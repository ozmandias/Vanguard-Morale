using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour {
    
    void Start() {
        GetFactionChoice();
    }

    void Update() {

    }

    void GetFactionChoice() {
        var kingdomDetails = GlobalData.kingdomDetails;
        var factionDetials = kingdomDetails.factionModel;

        CreateFaction(factionDetials);
    }

    void CreateFaction(FactionModel factionModel) {

    }

    void DetermineAllyOrEnemy() {

    }
}