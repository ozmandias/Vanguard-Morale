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
    public Vector3 []citizenCreatePoints;
    public Vector3 []soldierCreatePoints;
    public Vector3 []leaderCreatePoints;
    public Vector3 []itemCreatePoints;

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
                foreach(var citizenSpawnModel in factionModel.personSpawnModels) { // replace spawnModel.spawnPositions with spawnPoints of FactionManager set in scene (done)
                    for(int i = 0; i < citizenSpawnModel.spawnCount; i = i + 1) {
                        /*if(citizenSpawnModel.spawnPositions.Length == 0) {
                            citizenSpawnModel.spawnPositions = new Vector3[citizenCreatePoints.Length];
                            for(int j = 0; j < citizenCreatePoints.Length; j = j + 1) {
                                citizenSpawnModel.spawnPositions[j] = citizenCreatePoints[j];
                            }
                        }*/
                        var citizenObject = PersonManager.instance.CreatePerson(
                            citizenSpawnModel.spawnObject,
                            // citizenSpawnModel.spawnPositions[Random.Range(0, citizenSpawnModel.spawnPositions.Length)]
                            citizenCreatePoints[Random.Range(0, citizenCreatePoints.Length)]
                        );
                        factionCitizenList.Add(citizenObject.GetComponent<Citizen>());
                    }
                }
            }
            if(factionSoldierList.Count == 0) {
                factionSoldierList = FindObjectsOfType<Soldier>().ToList();
                foreach(var soldierSpawnModel in factionModel.soldierSpawnModels) { // replace spawnModel.spawnPositions with spawnPoints of FactionManager set in scene (done)
                    for(int i = 0; i < soldierSpawnModel.spawnCount; i = i + 1) {
                        /*if(soldierSpawnModel.spawnPositions.Length == 0) {
                            soldierSpawnModel.spawnPositions = new Vector3[soldierCreatePoints.Length];
                            for(int j = 0; j < soldierCreatePoints.Length; j = j + 1) {
                                soldierSpawnModel.spawnPositions[j] = soldierCreatePoints[j];
                            }
                        }*/
                        var soldierObject = PersonManager.instance.CreatePerson(
                            soldierSpawnModel.spawnObject,
                            // soldierSpawnModel.spawnPositions[Random.Range(0, soldierSpawnModel.spawnPositions.Length)]
                            soldierCreatePoints[Random.Range(0, soldierCreatePoints.Length)]
                        );
                        factionSoldierList.Add(soldierObject.GetComponent<Soldier>());
                    }
                }
            }
            if(factionLeaderList.Count == 0) {
                factionLeaderList = FindObjectsOfType<Leader>().ToList();
                foreach(var vanguardSpawnModel in factionModel.vanguardSpawnModels) { // replace spawnModel.spawnPositions with spawnPoints of FactionManager set in scene
                    for(int i = 0; i < vanguardSpawnModel.spawnCount; i = i + 1) {
                        /*if(vanguardSpawnModel.spawnPositions.Length == 0) {
                            vanguardSpawnModel.spawnPositions = new Vector3[leaderCreatePoints.Length];
                            for(int j = 0; j < leaderCreatePoints.Length; j = j + 1) {
                                vanguardSpawnModel.spawnPositions[j] = leaderCreatePoints[j];
                            }
                        }*/
                        var leaderObject = PersonManager.instance.CreatePerson(
                            vanguardSpawnModel.spawnObject,
                            // vanguardSpawnModel.spawnPositions[Random.Range(0, vanguardSpawnModel.spawnPositions.Length)]
                            leaderCreatePoints[Random.Range(0, leaderCreatePoints.Length)]
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
        var playerFaction = PlayerManager.instance.currentCharacter != null ? PlayerManager.instance.currentCharacter.faction : GlobalData.characterDetails.faction;
        var playerReputation = ReputationManager.instance.GetTwoFactionsReputation(currentFactionDetails.faction, playerFaction); // replace with 2 factions reputation (player faction, current faction)
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

    public Faction GetRandomFactionOfReputationList(List<ReputationModel> reputationModelList, string filter = "") {
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