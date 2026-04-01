using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : MonoBehaviour {
    public FactionScriptableObject factionScriptableObject;
    public FactionModel currentFactionDetails;
    public List<Citizen> factionCitizenList = new List<Citizen>();
    public List<Soldier> factionSoldierList = new List<Soldier>();
    public List<Boss> factionLeaderList = new List<Boss>();

    void Start() {
        GetFactionChoice();
    }

    void Update() {

    }

    void GetFactionChoice() {
        var kingdomDetails = GlobalData.kingdomDetails;
        currentFactionDetails = factionScriptableObject.dataList.Find((factionItem) => {
            return factionItem.faction == kingdomDetails.faction;
        });

        CreateFaction(currentFactionDetails);
    }

    void CreateFaction(FactionModel factionModel) {
        // if faction groups count is zero - spawn (or) pass data to Manager classes to spawn, else - manually put faction characters into scene (or) gameObjects manually register themselves
        // group all faction characters into arrays - people group, soldiers group, leaders group
        // create faction for only one time
        if(factionCitizenList.Count == 0 || factionSoldierList.Count == 0 || factionLeaderList.Count == 0) {
            if(factionCitizenList.Count == 0) {
                factionCitizenList = FindObjectsOfType<Citizen>().ToList();
            }
            if(factionSoldierList.Count == 0) {
                factionSoldierList = FindObjectsOfType<Soldier>().ToList();
            }
            if(factionLeaderList.Count == 0) {
                factionLeaderList = FindObjectsOfType<Boss>().ToList();
            }
        }

        // determine player as ally, neutral or enemy
        if(factionCitizenList.Count > 0 && factionSoldierList.Count > 0 && factionLeaderList.Count > 0) {
            DeterminePlayerAsAllyOrEnemy();
        }
    }

    void DeterminePlayerAsAllyOrEnemy() {
        // Check with ReputationManager
        var playerFaction = PlayerManager.instance.currentCharacter.faction;
        var playerReputation = ReputationManager.instance.GetPlayerReputation(playerFaction);
        switch(playerReputation) {
            case Reputation.Friendly:
                // turn all faction character groups to friends
                // shop prices with discount
                foreach(var factionCitizen in factionCitizenList) {
                    factionCitizen.GetInfo().personType = PersonType.Friend;
                }
                foreach(var factionSoldier in factionSoldierList) {
                    factionSoldier.GetInfo().personType = PersonType.Friend;
                }
                foreach(var factionLeader in factionLeaderList) {
                    factionLeader.GetInfo().personType = PersonType.Companion;
                }
                break;
            case Reputation.Neutral:
                // turn all faction character groups to neutral
                foreach(var factionCitizen in factionCitizenList) {
                    factionCitizen.GetInfo().personType = PersonType.Normal;
                }
                foreach(var factionSoldier in factionSoldierList) {
                    factionSoldier.GetInfo().personType = PersonType.Normal;
                }
                foreach(var factionLeader in factionLeaderList) {
                    factionLeader.GetInfo().personType = PersonType.Normal;
                }
                break;
            case Reputation.Hostile:
                // turn all faction character groups to enemies
                foreach(var factionCitizen in factionCitizenList) {
                    factionCitizen.GetInfo().personType = PersonType.Enemy;
                }
                foreach(var factionSoldier in factionSoldierList) {
                    factionSoldier.GetInfo().personType = PersonType.Enemy;
                }
                foreach(var factionLeader in factionLeaderList) {
                    factionLeader.GetInfo().personType = PersonType.Boss;
                }
                break;
            default:
                break;
        }
    }
}