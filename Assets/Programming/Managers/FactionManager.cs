// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : MonoBehaviour { // contains all factions and the faction of the Kingdom of current scene
    public FactionScriptableObject factionScriptableObject;
    public FactionModel currentFactionDetails;
    public List<Citizen> factionCitizenList = new List<Citizen>();
    public List<Soldier> factionSoldierList = new List<Soldier>();
    public List<Leader> factionLeaderList = new List<Leader>();
    public List<Item> factionItemList = new List<Item>();

    public static FactionManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        GetFactionChoice();
    }

    void Update() {

    }

    void GetFactionChoice() {
        var kingdomDetails = GlobalData.kingdomDetails;
        currentFactionDetails = factionScriptableObject.dataList.Find((factionData) => {
            return factionData.faction == kingdomDetails.faction;
        });

        CreateFaction(currentFactionDetails);
    }

    void CreateFaction(FactionModel factionModel) {
        // create faction for only one time using factionModel
        // create vanguards, special characters, person, soldier, guard, vendor, item using PersonManager
        // if faction groups count is zero - spawn (or) pass data to Manager classes to spawn, else - manually put faction characters into scene (or) gameObjects manually register themselves
        // group all faction characters into arrays - people group, soldiers group, leaders group
        if(factionCitizenList.Count == 0 || factionSoldierList.Count == 0 || factionLeaderList.Count == 0) {
            if(factionCitizenList.Count == 0) {
                factionCitizenList = FindObjectsOfType<Citizen>().ToList();
                foreach(var citizenSpawnModel in factionModel.personSpawnModels) {
                    for(int i = 0; i < citizenSpawnModel.spawnCount; i = i + 1) {
                        if(citizenSpawnModel.spawnPositions.Length == 0) {
                            citizenSpawnModel.spawnPositions = new Vector3[1];
                            citizenSpawnModel.spawnPositions[0] = Vector3.zero;
                        }
                        var citizenObject = PersonManager.instance.CreatePerson(
                            citizenSpawnModel.spawnObject,
                            citizenSpawnModel.spawnPositions[Random.Range(0, citizenSpawnModel.spawnPositions.Length)]
                        );
                        factionCitizenList.Add(citizenObject.GetComponent<Citizen>());
                    }
                }
            }
            if(factionSoldierList.Count == 0) {
                factionSoldierList = FindObjectsOfType<Soldier>().ToList();
                foreach(var soldierSpawnModel in factionModel.soldierSpawnModels) {
                    for(int i = 0; i < soldierSpawnModel.spawnCount; i = i + 1) {
                        if(soldierSpawnModel.spawnPositions.Length == 0) {
                            soldierSpawnModel.spawnPositions = new Vector3[1];
                            soldierSpawnModel.spawnPositions[0] = Vector3.zero;
                        }
                        var soldierObject = PersonManager.instance.CreatePerson(
                            soldierSpawnModel.spawnObject,
                            soldierSpawnModel.spawnPositions[Random.Range(0, soldierSpawnModel.spawnPositions.Length)]
                        );
                        factionSoldierList.Add(soldierObject.GetComponent<Soldier>());
                    }
                }
            }
            if(factionLeaderList.Count == 0) {
                factionLeaderList = FindObjectsOfType<Leader>().ToList();
                foreach(var vanguardSpawnModel in factionModel.vanguardSpawnModels) {
                    for(int i = 0; i < vanguardSpawnModel.spawnCount; i = i + 1) {
                        if(vanguardSpawnModel.spawnPositions.Length == 0) {
                            vanguardSpawnModel.spawnPositions = new Vector3[1];
                            vanguardSpawnModel.spawnPositions[0] = Vector3.zero;
                        }
                        var leaderObject = PersonManager.instance.CreatePerson(
                            vanguardSpawnModel.spawnObject,
                            vanguardSpawnModel.spawnPositions[Random.Range(0, vanguardSpawnModel.spawnPositions.Length)]
                        );
                        factionLeaderList.Add(leaderObject.GetComponent<Leader>());
                    }
                }
            }
        }

        // determine player as ally, neutral or enemy
        if(factionCitizenList.Count > 0 && factionSoldierList.Count > 0 && factionLeaderList.Count > 0) {
            DetermineReputationForPlayer(); // can improve Big (0) notation
        }
    }

    void DetermineReputationForPlayer() {
        // Check with ReputationManager
        var playerFaction = PlayerManager.instance.currentCharacter.faction;
        var playerReputation = ReputationManager.instance.GetOtherFactionReputation(playerFaction);
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

    public Faction GetRandomFactionOfReputationDetailsList(List<ReputationModel> reputationModelList, string filter = "") {
        List<ReputationModel> filterList = reputationModelList;
        switch(filter) {
            case "friend":
                filterList = reputationModelList.FindAll((reputationModel) => {
                    return reputationModel.reputation == Reputation.Friendly;
                });
                break;
            case "neutral":
                filterList = reputationModelList.FindAll((reputationModel) => {
                    return reputationModel.reputation == Reputation.Neutral;
                });
                break;
            case "enemy":
                filterList = reputationModelList.FindAll((reputationModel) => {
                    return reputationModel.reputation == Reputation.Hostile;
                });
                break;
            default:
                break;
        }
        return filterList[Random.Range(0, filterList.Count)].otherFaction;
    }

    public FactionModel GetFactionData(Faction faction) {
        var factionModel = factionScriptableObject.dataList.Find((factionData) => {
            return factionData.faction == faction;
        });
        return factionModel;
    }
}