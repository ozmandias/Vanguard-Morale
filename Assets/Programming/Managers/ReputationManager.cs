using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReputationManager : MonoBehaviour { // for each Kingdom; one ReputationManager is responsible for the Kingdom of the current scene
    public List<ReputationModel> reputationList;

    public static ReputationManager instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        reputationList = GlobalData.kingdomDetails.reputations.ToList();
    }

    public Reputation GetOtherFactionReputation(Faction otherFaction) {
        var otherFactionReputation = reputationList.Find((reputation) => {
            return reputation.otherFaction == otherFaction;
        });
        return otherFactionReputation.reputation;
    }

    public Reputation GetTwoFactionsReputation(Faction currentFaction, Faction otherFaction) {
        var currentFactionReputationList = WorldManager.instance.worldScriptableObject.dataList.Find((kingdomModel) => {
            return kingdomModel.faction == currentFaction;
        }).reputations.ToList();
        var otherFactionReputation = currentFactionReputationList.Find((reputation) => {
            return reputation.otherFaction == otherFaction;
        });
        Debug.Log("GetTwoFactionsReputation - currentFaction: " + currentFaction + ", otherFaction: " + otherFaction + ", reputation: " + otherFactionReputation.reputation);
        return otherFactionReputation.reputation;
    }

    public void CompareReputation(Faction currentFaction, Faction otherFaction) {

    }

    public List<ReputationModel> GetKingdomReputations(Faction kingdomFaction) {
        var kingdom = WorldManager.instance.worldScriptableObject.dataList.Find((kingdomModel) => {
            return kingdomModel.faction == kingdomFaction;
        });
        return kingdom.reputations.ToList();
    }
}